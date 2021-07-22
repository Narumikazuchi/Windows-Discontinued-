using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Narumikazuchi.Windows.Pipes
{
    /// <summary>
    /// Represents a server that handles named pipe connections with clients.
    /// </summary>
    [DebuggerDisplay("Name = {_pipeName}")]
    public sealed partial class NamedPipeServer<T> : IPipeSubscriber<T> where T : IByteConvertable<T>
    {
        #region Constructor

        /// <summary>
        /// Instantiates a new server with the specified pipe name.
        /// </summary>
        /// <param name="pipeName">The name for the named pipe connection.</param>
        /// <exception cref="ArgumentNullException"/>
        public NamedPipeServer([DisallowNull] String pipeName) : this(pipeName, 20) { }
        /// <summary>
        ///Instantiates a new server with the specified pipe name and the specified amount of max simultanous pipe instances.
        /// </summary>
        /// <param name="pipeName">The name for the named pipe connection.</param>
        /// <param name="maxInstances">The max amount of simultanous pipe instances.</param>
        /// <exception cref="ArgumentNullException"/>
        public NamedPipeServer([DisallowNull] String pipeName, Int32 maxInstances)
        {
            if (pipeName is null)
            {
                throw new ArgumentNullException(nameof(pipeName));
            }

            this._pipeName = pipeName;
            this._maxInstances = maxInstances;
        }

        #endregion

        #region Start/Stop

        /// <summary>
        /// Starts the server and begins waiting for connections.
        /// </summary>
        public void Start()
        {
            this._isRunning = true;
            this.CreateInstance();
        }

        /// <summary>
        /// Closes any open pipe and stops the server.
        /// </summary>
        public void Stop()
        {
            foreach (ServerPipe pipe in this._instances.Values)
            {
                pipe.Dispose();
            }
            this._instances.Clear();
            this._isRunning = false;
        }

        #endregion

        #region Data Processing

        private void ProcessIncomingData(Guid id, Byte[] data)
        {
            Int32 temp = 0;
            this.DataReceived?.Invoke(id, IByteConvertable<T>.ConvertFromBytes(data, ref temp));
        }

        private static Byte[] ProcessOutgoingData(T data) => data.ConvertToBytes();

        #endregion

        #region Data Sending

        /// <summary>
        /// Broadcasts the specified data to all connected clients.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentNullException"/>
        public void Send([DisallowNull] in T data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Byte[] result = ProcessOutgoingData(data);
            foreach (Guid id in this._instances.Keys)
            {
                this._instances[id].WriteBytes(result);
            }
        }

        /// <summary>
        /// Sends the specified data to the client with the specified id.
        /// </summary>
        /// <param name="clientId">The <see cref="Guid"/> of the client to send the data to.</param>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentNullException"/>
        public void Send(Guid clientId, [DisallowNull] in T data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Byte[] result = ProcessOutgoingData(data);
            this._instances[clientId].WriteBytes(result);
        }

        #endregion

        #region Pipe Management

        private void CreateInstance()
        {
            if (this._instances.Count < this._maxInstances)
            {
                ServerPipe pipe = new(this._pipeName);
                pipe.PipeConnected += (id) => {
                    this.ClientConnected?.Invoke();
                    this._instances.Add(id, pipe);
                    this.CreateInstance();
                };
                pipe.PipeClosed += () => {
                    this._instances.Remove(pipe.Id);
                    this.ClientDisconnected?.Invoke();
                };
                pipe.DataReceived += (b) => this.ProcessIncomingData(pipe.Id, b);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the server received data from the client with the given <see cref="Guid"/>.
        /// </summary>
        public event MappedDataReceivedEventHandler<Guid, T>? DataReceived;
        /// <summary>
        /// Occurs when a new client has connected to the server.
        /// </summary>
        public event Action? ClientConnected;
        /// <summary>
        /// Occurs when a client disconnected from the server.
        /// </summary>
        public event Action? ClientDisconnected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the server is currently running and actively handling or waiting for connections.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public Boolean IsRunning => this._isRunning;

        #endregion

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String _pipeName;
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private Int32 _maxInstances;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean _isRunning = false;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private Dictionary<Guid, ServerPipe> _instances = new();

        #endregion
    }
}
