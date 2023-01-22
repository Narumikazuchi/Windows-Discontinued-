namespace Narumikazuchi.Windows;

/// <summary>
/// Represents event data for the event where the windows theme gets changed.
/// </summary>
public sealed class ThemeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeChangedEventArgs"/> class.
    /// </summary>
    /// <param name="theme">The new windows theme.</param>
    public ThemeChangedEventArgs(WindowsTheme theme)
    {
        this.Theme = theme;
    }

    /// <summary>
    /// Gets the windows theme that has been switched to.
    /// </summary>
    public WindowsTheme Theme { get; }
}