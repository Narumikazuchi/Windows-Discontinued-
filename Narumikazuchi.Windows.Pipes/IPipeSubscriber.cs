namespace Narumikazuchi.Windows.Pipes
{
    /// <summary>
    /// Describes a generic named pipe wrapper.
    /// </summary>
    public interface IPipeSubscriber<TMessage> where TMessage : class, Serialization.Bytes.IByteSerializable
    {
        #region Start/Stop

        /// <summary>
        /// Starts the wrapper and initiates the connection.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops the wrapper and closes the connection.
        /// </summary>
        void Stop();

        #endregion

        #region Data Send

        /// <summary>
        /// Sends the specified data over the named pipe.
        /// </summary>
        /// <param name="data">The data to send over the named pipe.</param>
        void Send([System.Diagnostics.CodeAnalysis.DisallowNull] in TMessage data);

        #endregion
    }
}
