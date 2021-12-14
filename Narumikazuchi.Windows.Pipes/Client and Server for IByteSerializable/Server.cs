namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents an <see cref="IServer{TData}"/>, which communicates with <see cref="Client"/> objects through arrays of <see cref="Byte"/>s.
/// </summary>
public sealed partial class Server<TMessage>
    where TMessage : class, IByteSerializable
{
    /// <summary>
    /// Creates a new instance of the <see cref="Server"/> class.
    /// </summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="maxInstances">The maximum amount of clients that can be connected.</param>
    /// <param name="acceptCondition">The condition for a connection <see cref="Client"/> to be accepted.</param>
    /// <returns>A new <see cref="Server"/> instance with the specified parameters</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Server<TMessage> CreateServer([DisallowNull] String pipeName,
                                                in Int32 maxInstances,
                                                [DisallowNull] Func<Boolean> acceptCondition)
    {
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }
        if (acceptCondition is null)
        {
            throw new ArgumentNullException(nameof(acceptCondition));
        }
        if (maxInstances < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxInstances));
        }
        return new(pipeName,
                   maxInstances,
                   null,
                   acceptCondition);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Server"/> class.
    /// </summary>
    /// <param name="pipeName">The name of the pipe.</param>
    /// <param name="maxInstances">The maximum amount of clients that can be connected.</param>
    /// <param name="processor">The processor, who handles the incoming <see cref="Byte"/>[] objects.</param>
    /// <param name="acceptCondition">The condition for a connection <see cref="Client"/> to be accepted.</param>
    /// <returns>A new <see cref="Server"/> instance with the specified parameters</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Server<TMessage> CreateServer([DisallowNull] String pipeName,
                                                in Int32 maxInstances,
                                                [DisallowNull] ServerDataProcessor<TMessage> processor,
                                                [DisallowNull] Func<Boolean> acceptCondition)
    {
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }
        if (acceptCondition is null)
        {
            throw new ArgumentNullException(nameof(acceptCondition));
        }
        if (processor is null)
        {
            throw new ArgumentNullException(nameof(processor));
        }
        if (maxInstances < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxInstances));
        }
        return new(pipeName,
                   maxInstances,
                   processor,
                   acceptCondition);
    }
}

// Non-Public
partial class Server<TMessage>
{
    private Server([DisallowNull] String pipeName,
                   in Int32 maxInstances,
                   [AllowNull] IServerDataProcessor<TMessage>? processor,
                   [DisallowNull] Func<Boolean> acceptCondition) :
        base(pipeName,
             maxInstances,
             acceptCondition,
             processor)
    { }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ByteSerializer<TMessage> _serializer = new();
}

// ServerBase<Byte[]>
partial class Server<TMessage> : ServerBase<TMessage>
{
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    public override void Start() =>
        this.InitiateStart();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    public override void Stop() =>
        this.InitiateStop();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    public override async Task<Boolean> DisconnectAsync(Guid guid) =>
        await this.InitiateDisconnectAsync(guid);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="KeyNotFoundException"/>
    /// <exception cref="ObjectDisposedException"/>
    public override async Task SendAsync([DisallowNull] TMessage data,
                                    Guid client)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        await this.InitiateSendAsync(data,
                                     client);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ObjectDisposedException"/>
    public override async Task BroadcastAsync([DisallowNull] TMessage data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        await this.InitiateBroadcastAsync(data);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    protected override Byte[] SerializeToBytes([DisallowNull] TMessage data) =>
        data is null
            ? throw new ArgumentNullException(nameof(data))
            : this._serializer.Serialize(data);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    protected override TMessage SerializeFromBytes([DisallowNull] Byte[] bytes) =>
        bytes is null
            ? throw new ArgumentNullException(nameof(bytes))
            : this._serializer.Deserialize(bytes);
}