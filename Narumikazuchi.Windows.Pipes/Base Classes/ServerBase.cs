namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Provides the shared base functionality for sending data to multiple <see cref="IClient{TData}"/> objects.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract partial class ServerBase<TData>
{
    /// <summary>
    /// Gets whether the <see cref="IServer{TData}"/> allows connections from <see cref="IClient{TData}"/> objects.
    /// </summary>
    public Boolean IsRunning =>
        this._running;
}

// Non-Public
partial class ServerBase<TData>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServerBase{TData}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    protected ServerBase([DisallowNull] String pipeName,
                         in Int32 maxInstances,
                         [DisallowNull] Func<Boolean> acceptCondition,
                         [AllowNull] IServerDataProcessor<TData>? processor)
    {
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }

        this._name = pipeName;
        this._maxInstances = maxInstances;
        this._condition = acceptCondition;
        this._processor = processor;
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
    /// Initiates the connections accept and data receive pipeline.
    /// </summary>
    /// <exception cref="ObjectDisposedException"/>
    protected void InitiateStart()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ServerBase<TData>));
        }

        this._running = true;
        this.CreatePipeInstance();
    }

    /// <summary>
    /// Initiates and performs the disconnect from all sockets.
    /// </summary>
    /// <exception cref="ObjectDisposedException"/>
    protected void InitiateStop()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ServerBase<TData>));
        }

        foreach (__ServerPipe pipe in this._clients.Values)
        {
            try
            {
                pipe.Dispose();
            }
            catch (ObjectDisposedException) { }
        }
        this._clients.Clear();
        this._running = false;
        this.Dispose();
    }

    /// <summary>
    /// Initiates and performs the disconnect from the specified client socket.
    /// </summary>
    /// <exception cref="ObjectDisposedException"/>
    protected async Task<Boolean> InitiateDisconnectAsync(Guid guid)
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ServerBase<TData>));
        }

        if (!this._clients.ContainsKey(guid))
        {
            return false;
        }

        if (!this._clients[guid].Connected)
        {
            this._clients.Remove(guid);
            this.OnClientDisconnected(guid,
                                      ConnectionType.ConnectionLost);
            return true;
        }

        await this._clients[guid].WriteBytesAsync(_shutdownSignature);
        this._clients[guid].Close();
        this._clients.Remove(guid);
        this.OnClientDisconnected(guid,
                                  ConnectionType.ConnectionLost);
        return true;
    }

    /// <summary>
    /// Initiates the process for sending data over the socket connection to the specified client.
    /// </summary>
    /// <param name="data">The data to send to the client.</param>
    /// <param name="client">The guid associated with an <see cref="IClient{TData}"/>.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="KeyNotFoundException"/>
    /// <exception cref="ObjectDisposedException"/>
    protected async Task InitiateSendAsync([DisallowNull] TData data,
                                           Guid client)
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ServerBase<TData>));
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (!this._clients.ContainsKey(client))
        {
            throw new KeyNotFoundException();
        }
        Byte[] bytes = this.SerializeToBytes(data);
        await this._clients[client].WriteBytesAsync(bytes);
    }

    /// <summary>
    /// Initiates the process for sending data over the socket connection to the all connected clients.
    /// </summary>
    /// <param name="data">The data to send to the clients.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ObjectDisposedException"/>
    protected async Task InitiateBroadcastAsync([DisallowNull] TData data)
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException(nameof(ServerBase<TData>));
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        Byte[] bytes = this.SerializeToBytes(data);
        foreach (__ServerPipe pipe in this._clients.Values)
        {
            await pipe.WriteBytesAsync(bytes);
        }
    }

    private void CreatePipeInstance()
    {
        if (this._clients.Count < this._maxInstances)
        {
            __ServerPipe pipe = new(this._name);
            pipe.ConnectionEstablished += this.OnPipeConnected;
            pipe.ConnectionClosed += this.OnPipeClosed;
            pipe.DataReceived += this.ProcessIncomingData;
        }
    }

    private void OnPipeConnected(NamedPipe pipe, EventArgs e)
    {
        if (pipe is not __ServerPipe spipe)
        {
            throw new InvalidCastException();
        }

        Guid guid = Guid.NewGuid();
        this._clients.Add(guid,
                          spipe);
        spipe.WriteBytesAsync(_guidSignature.Concat(guid.ToByteArray())
                                            .ToArray());
        this.OnClientConnected(guid,
                               ConnectionType.ConnectionEstablished);
        this.CreatePipeInstance();
    }

    private void OnPipeClosed(NamedPipe pipe, EventArgs e)
    {
        if (pipe is not __ServerPipe spipe)
        {
            throw new InvalidCastException();
        }

        if (!this._clients.ContainsValue(spipe))
        {
            return;
        }

        Guid guid = this._clients.First(kv => kv.Value == spipe).Key;
        this._clients.Remove(guid);
        this.OnClientDisconnected(guid,
                                  ConnectionType.ConnectionClosed);
    }

    private void ProcessIncomingData(NamedPipe pipe,
                                     DataReceivedEventArgs<Byte[]> args)
    {
        if (pipe is null)
        {
            throw new ArgumentNullException(nameof(pipe));
        }
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }
        if (pipe is not __ServerPipe spipe)
        {
            throw new InvalidCastException();
        }
        if (args.Data is null)
        {
            throw new ArgumentException("No data present to process.");
        }
        if (!this._clients.ContainsValue(spipe))
        {
            throw new ArgumentException("Pipe was not associated with server.");
        }

        Guid guid = this._clients.First(kv => kv.Value == spipe).Key;
        if (args.Data.Length == 64)
        {
            if (args.Data.SequenceEqual(_shutdownSignature))
            {
                if (spipe.Connected)
                {
                    spipe.Close();
                }
                this._clients.Remove(guid);
                this.OnClientDisconnected(guid,
                                          ConnectionType.ConnectionLost);
                return;
            }
        }

        TData data = this.SerializeFromBytes(args.Data);
        if (this.DataProcessor is null)
        {
            this.DataReceived?.Invoke(this,
                                      new(data,
                                          guid));
            return;
        }
        this.DataProcessor.ProcessReceivedData(data,
                                               guid);
    }

    private void OnClientConnected(Guid client,
                                   ConnectionType connection) =>
        this.ClientConnected?.Invoke(this,
                                     new(client,
                                         connection));

    private void OnClientDisconnected(Guid client,
                                      ConnectionType connection) =>
        this.ClientDisconnected?.Invoke(this,
                                        new(client,
                                            connection));

    private readonly String _name;
    private readonly Int32 _maxInstances;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Dictionary<Guid, __ServerPipe> _clients = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IServerDataProcessor<TData>? _processor;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Func<Boolean> _condition;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Boolean _running;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Byte[] _shutdownSignature = new Byte[] { 0x72, 0x63, 0x53, 0x2E, 0x6D, 0x6E, 0x75, 0x74, 0x9C, 0x63, 0x5A, 0x78, 0x68, 0x2E, 0xBE, 0x67, 0xE4, 0x75, 0x69, 0x69, 0x6B, 0x65, 0x77, 0x74, 0x6F, 0x6B, 0xEE, 0x2E, 0x61, 0x4E, 0x77, 0x5, 0x61, 0xC2, 0x6B, 0x4E, 0x65, 0x73, 0xD1, 0x6F, 0x53, 0xF7, 0x7A, 0x86, 0x53, 0x68, 0x75, 0x74, 0x64, 0x6F, 0x77, 0x6E, 0x53, 0x65, 0x72, 0x65, 0x72, 0x43, 0x6C, 0x65, 0x68, 0x2E, 0xBE, 0x67 };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Byte[] _guidSignature = new Byte[] { 0x72, 0x63, 0x53, 0x2E, 0x6D, 0x6E, 0x75, 0x74, 0x9C, 0x63, 0x5A, 0x78, 0x68, 0x2E, 0xBE, 0x67, 0xE4, 0x75, 0x69, 0x69, 0x6B, 0x65, 0x77, 0x74, 0x6F, 0x6B, 0xEE, 0x2E, 0x61, 0x4E, 0x77, 0x5, 0x61, 0xC2, 0x6B, 0x4E, 0x65, 0x73, 0xD1, 0x6F, 0x53, 0xF7, 0x7A, 0x86, 0x47, 0x75, 0x69, 0x64 };
}

// IServer<TData>
partial class ServerBase<TData> : IServer<TData>
{
    /// <inheritdoc/>
    public abstract void Start();

    /// <inheritdoc/>
    public abstract void Stop();

    /// <inheritdoc/>
    public abstract Task<Boolean> DisconnectAsync(Guid guid);

    /// <inheritdoc/>
    public abstract Task SendAsync([DisallowNull] TData data,
                              Guid client);

    /// <inheritdoc/>
    public abstract Task BroadcastAsync([DisallowNull] TData data);

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    [NotNull]
    public IReadOnlyList<Guid> Clients =>
        this._disposed
            ? throw new ObjectDisposedException(nameof(ServerBase<TData>))
            : this._clients.Keys.ToList();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    [MaybeNull]
    public IServerDataProcessor<TData>? DataProcessor
    {
        get => this._disposed
                ? throw new ObjectDisposedException(nameof(ServerBase<TData>))
                : this._processor;
        set
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(ServerBase<TData>));
            }

            this._processor = value;
            if (this._processor is not null)
            {
                this._processor.Server = this;
            }
        }
    }

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    [NotNull]
    public Func<Boolean> AcceptCondition
    {
        get => this._disposed
                ? throw new ObjectDisposedException(nameof(ServerBase<TData>))
                : this._condition;
        set
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(ServerBase<TData>));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            this._condition = value;
        }
    }

    /// <inheritdoc/>
    public event EventHandler<IServer<TData>, ConnectionEventArgs>? ClientConnected;

    /// <inheritdoc/>
    public event EventHandler<IServer<TData>, ConnectionEventArgs>? ClientDisconnected;

    /// <inheritdoc/>
    public event EventHandler<IServer<TData>, DataReceivedEventArgs<TData>>? DataReceived;
}

// IDisposable
partial class ServerBase<TData> : IDisposable
{
    /// <inheritdoc/>
    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        if (this._running)
        {
            this.Stop();
        }

        foreach (__ServerPipe pipe in this._clients.Values)
        {
            try
            {
                pipe.Dispose();
            }
            catch (ObjectDisposedException) { }
        }

        this._disposed = true;
        GC.SuppressFinalize(this);
    }

    private Boolean _disposed = false;
}