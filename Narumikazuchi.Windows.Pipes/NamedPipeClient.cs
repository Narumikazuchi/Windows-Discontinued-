using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Narumikazuchi.Windows.Pipes
{
    /// <summary>
    /// Represents a client that handles named pipe connections with a server.
    /// </summary>
    [DebuggerDisplay("Guid = {Guid}")]
    public sealed partial class NamedPipeClient<T> : IPipeSubscriber<T> where T : IByteConvertable<T>
    {
        #region Constructor

        /// <summary>
        /// Instantiates a new client that can only connect to the sepcified server and specified named pipe.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public NamedPipeClient([DisallowNull] String serverNameOrIp, [DisallowNull] String pipeName)
        {
            if (serverNameOrIp is null)
            {
                throw new ArgumentNullException(nameof(serverNameOrIp));
            }
            if (pipeName is null)
            {
                throw new ArgumentNullException(nameof(pipeName));
            }

            this._server = serverNameOrIp;
            this._pipeName = pipeName;
        }

        #endregion

        #region Start/Stop

        /// <summary>
        /// Initiates the connection to the server.
        /// </summary>
        public void Start()
        {
            this._pipe = new ClientPipe(this._server, this._pipeName);
            this._pipe.PipeConnected += (id) => {
                this._id = id;
                this.Connected?.Invoke();
                this._isConnected = true;
            };
            this._pipe.PipeClosed += () => this.Disconnected?.Invoke();
            this._pipe.DataReceived += (b) => this.ProcessIncomingData(b);
            this._pipe.Connect();
        }

        /// <summary>
        /// Closes the connection to the server.
        /// </summary>
        public void Stop()
        {
            this._pipe?.Dispose();
            this._isConnected = false;
        }

        #endregion

        #region Sending Data

        /// <summary>
        /// Sends the specified data to the server.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="NotSupportedException"/>
        public void Send([DisallowNull] in T data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!this.IsConnected)
            {
                throw new NotSupportedException("The client is not connected.");
            }
            if (this._pipe is null)
            {
                throw new ArgumentException("The pipe couldn't be created or was null.");
            }

            Byte[] result = ProcessOutgoingData(data);
            this._pipe.WriteBytes(result);
        }

        #endregion

        #region Data Processing

        private void ProcessIncomingData(Byte[] data)
        {
            Int32 temp = 0;
            this.DataReceived?.Invoke(IByteConvertable<T>.ConvertFromBytes(data, ref temp));
        }

        private static Byte[] ProcessOutgoingData(T data) => data.ConvertToBytes();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the client received data from the server.
        /// </summary>
        public event DataReceivedEventHandler<T>? DataReceived;
        /// <summary>
        /// Occurs when the client connected to the server.
        /// </summary>
        public event Action? Connected;
        /// <summary>
        /// Occurs when the client disconnected from the server or when the client couldn't connect at all.
        /// </summary>
        public event Action? Disconnected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets if the client is currently connected to the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public Boolean IsConnected => this._isConnected;
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of this client.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Guid Id => this._id;

        #endregion

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private String _server;
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private String _pipeName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Boolean _isConnected = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _id;
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private ClientPipe? _pipe;

        #endregion
    }
}
