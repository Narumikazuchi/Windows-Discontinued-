namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Represents an object which process any <typeparamref name="TData"/> that the associated <see cref="IClient{TData}"/> receives.
/// </summary>
public interface IClientDataProcessor<TData>
{
    /// <summary>
    /// Process the specified <typeparamref name="TData"/> which this method recevied from it's associated <see cref="IClient{TData}"/>.
    /// </summary>
    /// <param name="data">The data received by the associated <see cref="IClient{TData}"/>.</param>
    public void ProcessReceivedData([System.Diagnostics.CodeAnalysis.DisallowNull] TData data);

    /// <summary>
    /// Disconnects the associated <see cref="IClient{TData}"/> from the <see cref="IServer{TData}"/> it is connected to.
    /// </summary>
    public void Disconnect();

    /// <summary>
    /// Gets or sets the associated <see cref="IClient{TData}"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.NotNull]
    public IClient<TData> Client { get; set; }
}