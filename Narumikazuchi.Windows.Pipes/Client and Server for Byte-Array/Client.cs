namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents an <see cref="IClient{TData}"/>, which communicates with <see cref="Server"/> objects through arrays of <see cref="Byte"/>.
/// </summary>
public sealed partial class Client
{
    /// <summary>
    /// Creates a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="serverNameOrIpAddress">The server name or ip address to connect to..</param>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static Client CreateClient([DisallowNull] String serverNameOrIpAddress,
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
    public static Client CreateClient([DisallowNull] String serverNameOrIpAddress,
                                      [DisallowNull] String pipeName,
                                      [DisallowNull] ClientDataProcessor processor)
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
partial class Client
{
    private Client([DisallowNull] String serverNameOrIpAddress,
                   [DisallowNull] String pipeName,
                   [AllowNull] IClientDataProcessor<Byte[]>? processor) :
        base(serverNameOrIpAddress,
             pipeName,
             processor)
    { }
}

// ClientBase<Byte[]>
partial class Client : ClientBase<Byte[]>
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
    public override async Task SendAsync([DisallowNull] Byte[] data)
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
    protected override Byte[] SerializeToBytes([DisallowNull] Byte[] data) =>
        data is null
            ? throw new ArgumentNullException(nameof(data))
            : data;

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    protected override Byte[] SerializeFromBytes([DisallowNull] Byte[] bytes) =>
        bytes is null
            ? throw new ArgumentNullException(nameof(bytes))
            : bytes;
}