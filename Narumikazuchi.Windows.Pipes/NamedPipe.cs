using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace Narumikazuchi.Windows.Pipes
{
    /// <summary>
    /// Implements the basic functionality of a named pipe.
    /// </summary>
    [DebuggerDisplay("Guid = {Guid}")]
    public abstract class NamedPipe : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Generates a <see cref="Guid"/> for this named pipe.
        /// </summary>
        protected NamedPipe() => this.Id = Guid.NewGuid();

        #endregion

        #region Stream Functions

        /// <summary>
        /// Clears the buffer for the named pipe and writes any data to the other end of the connection.
        /// </summary>
        public void Flush() => this.Pipe?.Flush();

        /// <summary>
        /// Performs a read operation for the underlying stream.
        /// </summary>
        /// <param name="onReceivedPacket">The method which handles the processing of the received bytes.</param>
        /// <exception cref="ArgumentNullException"/>
        protected void StartByteReaderAsync([DisallowNull] Action<Byte[]> onReceivedPacket)
        {
            if (onReceivedPacket is null)
            {
                throw new ArgumentNullException(nameof(onReceivedPacket));
            }

            if (this._isClosed ||
                this._isDisposed ||
                this.Pipe is null)
            {
                return;
            }

            Int32 size = sizeof(Int32);
            Byte[] lengthBuffer = new Byte[size];

            this.Pipe.ReadAsync(lengthBuffer, 0, size).ContinueWith(t => {
                Int32 readBytes = t.Result;
                if (readBytes == 0)
                {
                    this.PipeClosed?.Invoke();
                    this._isClosed = true;
                    return;
                }

                Int32 length = BitConverter.ToInt32(lengthBuffer, 0);
                Byte[] data = new Byte[length];

                this.Pipe.ReadAsync(data, 0, length).ContinueWith(ti => {
                    Int32 readBytes2 = ti.Result;
                    if (readBytes2 == 0)
                    {
                        this.PipeClosed?.Invoke();
                        this._isClosed = true;
                        return;
                    }
                    onReceivedPacket(data);
                    this.StartByteReaderAsync(onReceivedPacket);
                });
            });
        }

        /// <summary>
        /// Writes the specified data to the pipe.
        /// </summary>
        /// <param name="bytes">The data to write.</param>
        /// <exception cref="ArgumentNullException"/>
        public Task WriteBytes([DisallowNull] Byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (this._isClosed ||
                this._isDisposed ||
                this.Pipe is null)
            {
                return Task.CompletedTask;
            }
            Byte[] dataLength = BitConverter.GetBytes(bytes.Length);
            Byte[] completeData = dataLength.Concat(bytes).ToArray();
            return this.Pipe.WriteAsync(completeData, 0, completeData.Length);
        }

        /// <summary>
        /// Initiates the read process for the named pipe. The read process will only stop once the pipe was closed.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public void StartReaderAsync() => this.StartByteReaderAsync(b => this.DataReceived?.Invoke(b));

        #endregion

        #region Close

        /// <summary>
        /// Closes the pipe connection.
        /// </summary>
        public void Close()
        {
            if (this._isDisposed)
            {
                return;
            }
            this.Pipe?.WaitForPipeDrain();
            this.Pipe?.Close();
            this._isClosed = true;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Closes the pipe connection and disposes any unmanged resouces used by this object.
        /// </summary>
        public void Dispose()
        {
            if (this._isDisposed)
            {
                return;
            }
            this.Close();
            this.Pipe?.Dispose();
            this.Pipe = null;
            this._isDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the pipe connected.
        /// </summary>
        public event Action<Guid>? PipeConnected;
        /// <summary>
        /// Occurs when the pipe disconnected or couldn't connect at all.
        /// </summary>
        public event Action? PipeClosed;
        /// <summary>
        /// Occurs when the pipe received data over it's connection.
        /// </summary>
        public event Action<Byte[]>? DataReceived;

        /// <summary>
        /// Triggers the <see cref="PipeConnected"/> event for this pipe.
        /// </summary>
        protected void OnConnected()
        {
            this._isClosed = false;
            this.PipeConnected?.Invoke(this.Id);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique <see cref="Guid"/> of this pipe.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Guid Id { get; }

        /// <summary>
        /// The underlying <see cref="PipeStream"/> for this named pipe.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        protected PipeStream? Pipe { get; set; }

        #endregion

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private Boolean _isClosed = true;
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        private Boolean _isDisposed = false;

        #endregion
    }
}
