namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents the base functionality of a pipe server.
/// </summary>
public interface IServer<TData> : System.IDisposable
{
    /// <summary>
    /// Starts the <see cref="IServer{TData}"/> and enables <see cref="IClient{TData}"/> objects to connect to it.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stops the <see cref="IServer{TData}"/> and closes the connection to all connected <see cref="IClient{TData}"/> objects.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Disconnects the <see cref="IClient{TData}"/> who is associated with the specified <see cref="System.Guid"/>.
    /// </summary>
    /// <param name="guid">The guid the client is associated with.</param>
    /// <returns><see langword="true"/> if the client was disconnected; otherwise, <see langword="false"/></returns>
    public System.Threading.Tasks.Task<System.Boolean> DisconnectAsync(System.Guid guid);

    /// <summary>
    /// Sends the specified <typeparamref name="TData"/> to the <see cref="IClient{TData}"/> associated with the specified <see cref="System.Guid"/>.
    /// </summary>
    /// <param name="data">The data to send.</param>
    /// <param name="client">The guid the client is associated with.</param>
    public System.Threading.Tasks.Task SendAsync([System.Diagnostics.CodeAnalysis.DisallowNull] TData data,
                                                 System.Guid client);

    /// <summary>
    /// Sends the specified <typeparamref name="TData"/> to the all connected <see cref="IClient{TData}"/> objects.
    /// </summary>
    /// <param name="data">The data to send.</param>
    public System.Threading.Tasks.Task BroadcastAsync([System.Diagnostics.CodeAnalysis.DisallowNull] TData data);

    /// <summary>
    /// Occurs when a new <see cref="IClient{TData}"/> connected with the <see cref="IServer{TData}"/>.
    /// </summary>
    public event EventHandler<IServer<TData>, ConnectionEventArgs>? ClientConnected;

    /// <summary>
    /// Occurs when an <see cref="IClient{TData}"/> has been disconnected from the <see cref="IServer{TData}"/>, either through <see cref="DisconnectAsync(System.Guid)"/> or from the client-side.
    /// </summary>
    public event EventHandler<IServer<TData>, ConnectionEventArgs>? ClientDisconnected;

    /// <summary>
    /// Occurs when the <see cref="DataProcessor"/> property is set to <see langword="null"/> and the <see cref="IServer{TData}"/> receives <typeparamref name="TData"/> from one of it's connected <see cref="IClient{TData}"/> objects.
    /// </summary>
    /// <remarks>
    /// This event will never get raised, if the <see cref="DataProcessor"/> property is set to a not-null object.
    /// </remarks>
    public event EventHandler<IServer<TData>, DataReceivedEventArgs<TData>>? DataReceived;

    /// <summary>
    /// Gets or sets the condition for a new <see cref="IClient{TData}"/> connection to be accepted.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.NotNull]
    public System.Func<System.Boolean> AcceptCondition { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IServerDataProcessor{TData}"/> for this <see cref="IServer{TData}"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="IServerDataProcessor{TData}"/> provides the <see cref="IServer{TData}"/> with the functionality to process incoming data.
    /// If this property is not set, the <see cref="IServer{TData}"/> will instead raise the <see cref="DataReceived"/> event every time new data will be received.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.MaybeNull]
    public IServerDataProcessor<TData>? DataProcessor { get; set; }

    /// <summary>
    /// Gets all associated <see cref="System.Guid"/> for the currently connected <see cref="IClient{TData}"/> objects.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.NotNull]
    public System.Collections.Generic.IReadOnlyList<System.Guid> Clients { get; }
}