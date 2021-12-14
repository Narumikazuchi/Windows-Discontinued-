namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents the base functionality of a pipe client.
/// </summary>
public interface IClient<TData> : System.IDisposable
{
    /// <summary>
    /// Connects the <see cref="IClient{TData}"/> to the pipe on the maschine with the <see cref="IServer{TData}"/>.
    /// </summary>
    public void Connect();

    /// <summary>
    /// Disconnects the <see cref="IClient{TData}"/> from it's current connection to an <see cref="IServer{TData}"/>.
    /// </summary>
    public void Disconnect();

    /// <summary>
    /// Sends the specified <typeparamref name="TData"/> to the connected <see cref="IServer{TData}"/>.
    /// </summary>
    /// <param name="data">The data to send.</param>
    public System.Threading.Tasks.Task SendAsync([System.Diagnostics.CodeAnalysis.DisallowNull] TData data);

    /// <summary>
    /// Occurs when the connection to an <see cref="IServer{TData}"/> has been closed, either client-side or server-side.
    /// </summary>
    public event EventHandler<IClient<TData>>? ConnectionClosed;

    /// <summary>
    /// Occurs when the connection to an <see cref="IServer{TData}"/> has been successfully established.
    /// </summary>
    public event EventHandler<IClient<TData>>? ConnectionEstablished;

    /// <summary>
    /// Occurs when the <see cref="DataProcessor"/> property is set to <see langword="null"/> and the <see cref="IClient{TData}"/> receives <typeparamref name="TData"/> from it's connected <see cref="IServer{TData}"/>.
    /// </summary>
    /// <remarks>
    /// This event will never get raised, if the <see cref="DataProcessor"/> property is set to a not-null object.
    /// </remarks>
    public event EventHandler<IClient<TData>, DataReceivedEventArgs<TData>>? DataReceived;

    /// <summary>
    /// Gets the <see cref="System.Guid"/> that this <see cref="IClient{TData}"/> is assigned to on it's connected <see cref="IServer{TData}"/>.
    /// </summary>
    public System.Guid Guid { get; }

    /// <summary>
    /// Gets or sets the <see cref="IClientDataProcessor{TData}"/> for this <see cref="IClient{TData}"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="IClientDataProcessor{TData}"/> provides the <see cref="IClient{TData}"/> with the functionality to process incoming data.
    /// If this property is not set, the <see cref="IClient{TData}"/> will instead raise the <see cref="DataReceived"/> event every time new data will be received.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.MaybeNull]
    public IClientDataProcessor<TData>? DataProcessor { get; set; }
}