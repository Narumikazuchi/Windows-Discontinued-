namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents an object which process any <typeparamref name="TData"/> that the associated <see cref="IServer{TData}"/> receives.
/// </summary>
public interface IServerDataProcessor<TData>
{
    /// <summary>
    /// Process the specified <typeparamref name="TData"/> which this method recevied from it's associated <see cref="IServer{TData}"/>.
    /// </summary>
    /// <param name="data">The data received by the associated <see cref="IServer{TData}"/>.</param>
    /// <param name="fromClient">The guid of the associated <see cref="IClient{TData}"/>.</param>
    public void ProcessReceivedData([System.Diagnostics.CodeAnalysis.DisallowNull] TData data,
                                    in System.Guid fromClient);

    /// <summary>
    /// Disconnects the <see cref="IClient{TData}"/> associated with the specified <see cref="System.Guid"/> from the associated <see cref="IServer{TData}"/>.
    /// </summary>
    public void DisconnectClient(in System.Guid client);

    /// <summary>
    /// Gets or sets the associated <see cref="IServer{TData}"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.NotNull]
    public IServer<TData> Server { get; set; }
}