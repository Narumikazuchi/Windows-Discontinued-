namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Provides the shared base functionality for sending data to an <see cref="IServer{TData}"/>.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract partial class ClientBase<TData>
{
    /// <summary>
    /// Gets whether the <see cref="IClient{TData}"/> is connected to an <see cref="IServer{TData}"/> at the moment.
    /// </summary>
    /// <exception cref="ObjectDisposedException"/>
    public Boolean Connected =>
        this._disposed
            ? throw new ObjectDisposedException(nameof(ClientBase<TData>))
            : !this._guid.Equals(default) &&
              this._pipe.Connected;
}

// Non-Public
partial class ClientBase<TData>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientBase{TData}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    protected ClientBase([DisallowNull] String serverNameOrIpAddress,
                         [DisallowNull] String pipeName,
                         [AllowNull] IClientDataProcessor<TData>? processor)
    {
        if (serverNameOrIpAddress is null)
        {
            throw new ArgumentNullException(nameof(serverNameOrIpAddress));
        }
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }

        this._name = pipeName;
        this._address = serverNameOrIpAddress;
        this._processor = processor;
        this._pipe = new(serverNameOrIpAddress,
                         pipeName);
    }

    /// <summary>
    /// Serializes the specified <typeparamref name="TData"/> into a <see cref="Byte"/>[] representation, which in turn can be send over the pipe connection.
    /// </summary>
    /// <param name="data">The data to serialize.</param>
    /// <returns>The <see cref="Byte"/>[] representation of the specified <typeparamref name="TData"/></returns>
    [return: NotNull]
    protected abstract Byte[] SerializeToBytes([DisallowNull] TData data);

    /// <summary>
    /// Serializes the specified <see cref="Byte"/>[] representation back into it's <typeparamref name="TData"/> object.
    /// </summary>
    /// <param name="bytes">The <see cref="Byte"/>[] representation to serialize.</param>
    /// <returns>The <typeparamref name="TData"/> object serialized from the specified <see cref="Byte"/>[] representation</returns>
    [return: NotNull]
    protected abstract TData SerializeFromBytes([DisallowNull] Byte[] bytes);

    /// <summary>
    /// Initiates the data receive pipeline.
    /// </summary>
    /// <exception cref="ObjectDisposedException"/>
    protected void InitiateConnection()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ClientBase<TData>));
        }

        this._pipe.ConnectionClosed += (s, e) => this.OnConnectionClosed();
        this._pipe.DataReceived += this.ProcessIncomingData;
        this._pipe.Connect();
    }

    /// <summary>
    /// Initiates and performs the disconnect from the server socket.
    /// </summary>
    /// <param name="raiseEvent">Whether or not the <see cref="IClient{TData}.ConnectionClosed"/> event shall be raised.</param>
    /// <exception cref="ObjectDisposedException"/>
    protected void InitiateDisconnect(in Boolean raiseEvent)
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ClientBase<TData>));
        }

        this._pipe.Close();
    }

    /// <summary>
    /// Initiates the process for sending data over the socket connection to the server.
    /// </summary>
    /// <param name="data">The data to send to the server.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ObjectDisposedException"/>
    protected async Task InitiateSendAsync([DisallowNull] TData data)
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ClientBase<TData>));
        }
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        Byte[] bytes = this.SerializeToBytes(data);
        await this._pipe.WriteBytesAsync(bytes);
    }

    private void ProcessIncomingData(NamedPipe pipe,
                                     DataReceivedEventArgs<Byte[]> args)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }
        if (args.Data is null)
        {
            throw new ArgumentException("");
        }

        if (args.Data.Length == 64)
        {
            if (args.Data.SequenceEqual(_shutdownSignature))
            {
                this.InitiateDisconnect(true);
                return;
            }
            if (args.Data.Take(48)
                         .SequenceEqual(_guidSignature))
            {
                Byte[] guidBytes = args.Data.Skip(48)
                                            .Take(16)
                                            .ToArray();
                this._guid = new(guidBytes);
                this.OnConnectionEstablished();
                return;
            }
        }

        TData data = this.SerializeFromBytes(args.Data);
        if (this.DataProcessor is null)
        {
            this.DataReceived?.Invoke(this,
                                      new(data));
            return;
        }
        this.DataProcessor.ProcessReceivedData(data);
    }

    private void OnConnectionEstablished() =>
        this.ConnectionEstablished?.Invoke(this,
                                           EventArgs.Empty);

    private void OnConnectionClosed() =>
        this.ConnectionClosed?.Invoke(this,
                                      EventArgs.Empty);

    private readonly __ClientPipe _pipe;
    private readonly String _name;
    private readonly String _address;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IClientDataProcessor<TData>? _processor;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Guid _guid = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Byte[] _shutdownSignature = new Byte[] { 0x72, 0x63, 0x53, 0x2E, 0x6D, 0x6E, 0x75, 0x74, 0x9C, 0x63, 0x5A, 0x78, 0x68, 0x2E, 0xBE, 0x67, 0xE4, 0x75, 0x69, 0x69, 0x6B, 0x65, 0x77, 0x74, 0x6F, 0x6B, 0xEE, 0x2E, 0x61, 0x4E, 0x77, 0x5, 0x61, 0xC2, 0x6B, 0x4E, 0x65, 0x73, 0xD1, 0x6F, 0x53, 0xF7, 0x7A, 0x86, 0x53, 0x68, 0x75, 0x74, 0x64, 0x6F, 0x77, 0x6E, 0x53, 0x65, 0x72, 0x65, 0x72, 0x43, 0x6C, 0x65, 0x68, 0x2E, 0xBE, 0x67 };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Byte[] _guidSignature = new Byte[] { 0x72, 0x63, 0x53, 0x2E, 0x6D, 0x6E, 0x75, 0x74, 0x9C, 0x63, 0x5A, 0x78, 0x68, 0x2E, 0xBE, 0x67, 0xE4, 0x75, 0x69, 0x69, 0x6B, 0x65, 0x77, 0x74, 0x6F, 0x6B, 0xEE, 0x2E, 0x61, 0x4E, 0x77, 0x5, 0x61, 0xC2, 0x6B, 0x4E, 0x65, 0x73, 0xD1, 0x6F, 0x53, 0xF7, 0x7A, 0x86, 0x47, 0x75, 0x69, 0x64 };
}

// IClient<TData>
partial class ClientBase<TData> : IClient<TData>
{
    /// <inheritdoc/>
    public abstract void Connect();

    /// <inheritdoc/>
    public abstract void Disconnect();

    /// <inheritdoc/>
    public abstract Task SendAsync([DisallowNull] TData data);

    /// <inheritdoc/>
    public Guid Guid => this._guid;

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    [MaybeNull]
    public IClientDataProcessor<TData>? DataProcessor
    {
        get => this._disposed
                ? throw new ObjectDisposedException(nameof(ClientBase<TData>))
                : this._processor;
        set
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(ClientBase<TData>));
            }

            this._processor = value;
            if (this._processor is not null)
            {
                this._processor.Client = this;
            }
        }
    }

    /// <inheritdoc/>
    public event EventHandler<IClient<TData>>? ConnectionEstablished;

    /// <inheritdoc/>
    public event EventHandler<IClient<TData>>? ConnectionClosed;

    /// <inheritdoc/>
    public event EventHandler<IClient<TData>, DataReceivedEventArgs<TData>>? DataReceived;
}

// IDisposable
partial class ClientBase<TData> : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        try
        {
            this._pipe.Dispose();
        }
        catch (ObjectDisposedException) { }
        catch (InvalidOperationException) { }
        this._disposed = true;
        GC.SuppressFinalize(this);
    }

    private Boolean _disposed = false;
}