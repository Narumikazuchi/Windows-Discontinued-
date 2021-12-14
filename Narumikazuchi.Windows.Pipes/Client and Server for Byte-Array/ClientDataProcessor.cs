namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Provides the blueprint for data processing of an <see cref="IClient{TData}"/>.
/// </summary>
// Non-Public
public abstract partial class ClientDataProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientDataProcessor"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    protected ClientDataProcessor([DisallowNull] Client client)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        this._client = client;
        if (this._client.DataProcessor != this)
        {
            this._client.DataProcessor = this;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IClient<Byte[]> _client;
}

// IClientDataProcessor<Byte[]>
partial class ClientDataProcessor : IClientDataProcessor<Byte[]>
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    public abstract void ProcessReceivedData([DisallowNull] Byte[] data);

    /// <summary>
    /// Disconnects the <see cref="Client"/> from the <see cref="Server"/>.
    /// </summary>
    public void Disconnect() =>
        this.Client.Disconnect();

    /// <summary>
    /// Gets or sets the <see cref="Client"/> associated with this processor.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [NotNull]
    public IClient<Byte[]> Client
    {
        get => this._client;
        set
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            this._client = value;
            if (this._client.DataProcessor != this)
            {
                this._client.DataProcessor = this;
            }
        }
    }
}