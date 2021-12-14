namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Contains the <typeparamref name="TData"/> that has been received by either an <see cref="IClient{TData}"/> or an <see cref="IServer{TData}"/>.
/// </summary>
public sealed class DataReceivedEventArgs<TData> : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataReceivedEventArgs{TData}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    public DataReceivedEventArgs([DisallowNull] TData data,
                                 in Guid fromClient)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        this.Data = data;
        this.FromClient = fromClient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataReceivedEventArgs{TData}"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    public DataReceivedEventArgs([DisallowNull] TData data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        this.Data = data;
        this.FromClient = null;
    }

    /// <summary>
    /// Gets the <see cref="Guid"/> of the <see cref="IClient{TData}"/> the was received from. This is <see langword="null"/> if the <typeparamref name="TData"/> was received from an <see cref="IServer{TData}"/>.
    /// </summary>
    [MaybeNull]
    public Guid? FromClient { get; }

    /// <summary>
    /// Gets the <typeparamref name="TData"/> that has been received.
    /// </summary>
    [NotNull]
    public TData Data { get; }
}