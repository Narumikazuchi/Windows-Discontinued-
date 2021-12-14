namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Implements the basic functionality of a named pipe.
/// </summary>
public abstract partial class NamedPipe
{
    /// <summary>
    /// Clears the buffer for the named pipe and writes any data to the other end of the connection.
    /// </summary>
    public void Flush() =>
        this.Pipe?.Flush();

    /// <summary>
    /// Writes the specified data to the pipe.
    /// </summary>
    /// <param name="bytes">The data to write.</param>
    /// <exception cref="ArgumentNullException"/>
    public ValueTask WriteBytesAsync([DisallowNull] Byte[] bytes)
    {
        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        if (this._closed ||
            this._disposed ||
            this.Pipe is null)
        {
            return ValueTask.CompletedTask;
        }
        Byte[] dataLength = BitConverter.GetBytes(bytes.Length);
        Byte[] completeData = dataLength.Concat(bytes)
                                        .ToArray();
        return this.Pipe.WriteAsync(completeData.AsMemory(0,
                                                         completeData.Length));
    }

    /// <summary>
    /// Initiates the read process for the named pipe. The read process will only stop once the pipe was closed.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    public Task StartReaderAsync() =>
        this.StartByteReaderAsync();

    /// <summary>
    /// Closes the pipe connection.
    /// </summary>
    public void Close()
    {
        if (this._disposed)
        {
            return;
        }
        this.Pipe?.WaitForPipeDrain();
        this.Pipe?.Close();
        this._closed = true;
    }

    /// <summary>
    /// Gets whether this pipe is currently connected.
    /// </summary>
    public Boolean Connected => this._connected;

    /// <summary>
    /// Occurs when the pipe connected.
    /// </summary>
    public event EventHandler<NamedPipe>? ConnectionEstablished;

    /// <summary>
    /// Occurs when the pipe disconnected or couldn't connect at all.
    /// </summary>
    public event EventHandler<NamedPipe>? ConnectionClosed;

    /// <summary>
    /// Occurs when the pipe received data over it's connection.
    /// </summary>
    public event EventHandler<NamedPipe, DataReceivedEventArgs<Byte[]>>? DataReceived;
}

// Non-Public
partial class NamedPipe
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedPipe"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    protected NamedPipe([DisallowNull] PipeStream pipe)
    {
        if (pipe is null)
        {
            throw new ArgumentNullException(nameof(pipe));
        }

        this._pipe = pipe;
    }

    /// <summary>
    /// Performs a read operation for the underlying stream.
    /// </summary>
    protected async Task StartByteReaderAsync()
    {
        if (this._closed ||
            this._disposed)
        {
            return;
        }

        Byte[] lengthBuffer = new Byte[4];

        Int32 read = await this.Pipe.ReadAsync(lengthBuffer.AsMemory(0,
                                                                     4));
        if (read == 0)
        {
            return;
        }

        Int32 length = BitConverter.ToInt32(lengthBuffer,
                                            0);
        Byte[] data = new Byte[length];
        read = await this.Pipe.ReadAsync(data.AsMemory(0,
                                                       length));
        if (read == 0)
        {
            return;
        }
        this.DataReceived?.Invoke(this,
                                  new(data));
        await this.StartByteReaderAsync();
    }

    /// <summary>
    /// Triggers the <see cref="ConnectionEstablished"/> event for this pipe.
    /// </summary>
    protected void OnConnected()
    {
        this._closed = false;
        this.ConnectionEstablished?.Invoke(this,
                                   EventArgs.Empty);
    }

    /// <summary>
    /// The underlying <see cref="PipeStream"/> for this named pipe.
    /// </summary>
    [NotNull]
    protected PipeStream Pipe => this._pipe;

    /// <summary>
    /// Backing field for <see cref="Connected"/> property.
    /// </summary>
    protected Boolean _connected = false;

    private readonly PipeStream _pipe;
    private Boolean _closed = true;
}

// IDisposable
partial class NamedPipe : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        this.Close();
        this.Pipe?.Dispose();
        this._disposed = true;
        GC.SuppressFinalize(this);
    }

    private Boolean _disposed = false;
}