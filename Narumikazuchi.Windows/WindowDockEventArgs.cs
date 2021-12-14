namespace Narumikazuchi.Windows;

/// <summary>
/// Contains the new <see cref="WindowDockPosition"/> of the associated window.
/// </summary>
public sealed class WindowDockEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowDockEventArgs"/> class.
    /// </summary>
    public WindowDockEventArgs(in WindowDockPosition dockPosition) =>
        this.DockPosition = dockPosition;

    /// <summary>
    /// Gets the <see cref="WindowDockPosition"/> of the associated window.
    /// </summary>
    [Pure]
    public WindowDockPosition DockPosition { get; }
}