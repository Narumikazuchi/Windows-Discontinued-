namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Contains the connected or disconnected client.
/// </summary>
public sealed partial class ConnectionEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionEventArgs"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    public ConnectionEventArgs(Guid whichClient,
                               in ConnectionType type)
    {
        this._client = whichClient;
        this.EventType = type;
    }

    /// <summary>
    /// Gets or sets the <see cref="IClient{TData}"/> that connected/disconnected.
    /// </summary>
    public Guid Client
    {
        get => _client;
        set => this._client = value;
    }

    /// <summary>
    /// Gets or sets if the connection was established, closed or lost.
    /// </summary>
    public ConnectionType EventType { get; set; }
}

// Non-Public
partial class ConnectionEventArgs
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Guid _client;
}