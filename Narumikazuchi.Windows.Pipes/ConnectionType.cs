namespace Narumikazuchi.Windows.Pipes;

/// <summary>
/// Specifies the <see cref="ConnectionType"/> that occurred.
/// </summary>
public enum ConnectionType
{
    /// <summary>
    /// A connection was established.
    /// </summary>
    ConnectionEstablished = 1,

    /// <summary>
    /// A connection was closed.
    /// </summary>
    ConnectionClosed = 2,

    /// <summary>
    /// A connection was lost.
    /// </summary>
    ConnectionLost = 3
}