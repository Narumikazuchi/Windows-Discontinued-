namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents an <see cref="IClient{TData}"/>, which communicates with <see cref="Server"/> objects through arrays of <see cref="Byte"/>.
/// </summary>
public sealed partial class Client<TMessage>
    where TMessage : class, IByteSerializable
{
    /// <summary>
    /// Creates a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="serverNameOrIpAddress">The server name or ip address to connect to..</param>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Client<TMessage> CreateClient([DisallowNull] String serverNameOrIpAddress,
                                                [DisallowNull] String pipeName)
    {
        if (serverNameOrIpAddress is null)
        {
            throw new ArgumentNullException(nameof(serverNameOrIpAddress));
        }
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }

        return new(serverNameOrIpAddress,
                   pipeName,
                   null);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="serverNameOrIpAddress">The server name or ip address to connect to..</param>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <param name="processor">The processor, who handles the incoming <see cref="Byte"/>[] objects.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Client<TMessage> CreateClient([DisallowNull] String serverNameOrIpAddress,
                                                [DisallowNull] String pipeName,
                                                [DisallowNull] ClientDataProcessor<TMessage> processor)
    {
        if (serverNameOrIpAddress is null)
        {
            throw new ArgumentNullException(nameof(serverNameOrIpAddress));
        }
        if (pipeName is null)
        {
            throw new ArgumentNullException(nameof(pipeName));
        }
        if (processor is null)
        {
            throw new ArgumentNullException(nameof(processor));
        }

        return new(serverNameOrIpAddress,
                   pipeName,
                   processor);
    }
}

// Non-Public
partial class Client<TMessage>
{
    private Client([DisallowNull] String serverNameOrIpAddress,
                   [DisallowNull] String pipeName,
                   [AllowNull] IClientDataProcessor<TMessage>? processor) :
        base(serverNameOrIpAddress,
             pipeName,
             processor)
    { }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ByteSerializer<TMessage> _serializer = new();
}

// ClientBase<Byte[]>
partial class Client<TMessage> : ClientBase<TMessage>
{
    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    public override void Connect() =>
        this.InitiateConnection();

    /// <inheritdoc/>
    /// <exception cref="ObjectDisposedException"/>
    public override void Disconnect() =>
        this.InitiateDisconnect(true);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ObjectDisposedException"/>
    public override async Task SendAsync([DisallowNull] TMessage data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await this.InitiateSendAsync(data);
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