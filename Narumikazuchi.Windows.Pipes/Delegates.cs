namespace Narumikazuchi.Windows.Pipes
{
    /// <summary>
    /// Represents the method which will handle a data received event.
    /// </summary>
    /// <param name="data">The received data.</param>
    public delegate void DataReceivedEventHandler<T>(T data) where T : IByteConvertable<T>;
    /// <summary>
    /// Represents the method which will handle a data received event, included the mapped key for the pipe that send it.
    /// </summary>
    /// <param name="key">The key of the mapped sender pipe.</param>
    /// <param name="data">The received data.</param>
    public delegate void MappedDataReceivedEventHandler<TKey, TData>(TKey key, TData data) where TData : IByteConvertable<TData>;
}
