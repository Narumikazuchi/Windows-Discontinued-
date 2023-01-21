namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Fixes the maximize issue with <see cref="WindowStyle.None"/> covering the taskbar.
/// </summary>
public sealed partial class BorderlessWindowResizer
{
    /// <summary>
    /// Attaches a new instance of <see cref="BorderlessWindowResizer"/> to the <see cref="Window"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    static public void AttachTo([DisallowNull] Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        s_Attached.Add(key: window,
                       value: new(window));
    }

    /// <summary>
    /// Gets the <see cref="BorderlessWindowResizer"/> attached to the specified <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to look up.</param>
    /// <param name="resizer">The <see cref="BorderlessWindowResizer"/> for the specified window or <see langword="null"/> if the window is not associated with a resizer.</param>
    /// <returns><see langword="true"/> if a <see cref="BorderlessWindowResizer"/> is attached to the specified window; otherwise, <see langword="false"/></returns>
    [Pure]
    [return: MaybeNull]
    static public Boolean TryGetResizerForWindow([DisallowNull] Window window,
                                                 [NotNullWhen(true)] out BorderlessWindowResizer? resizer)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (!s_Attached.ContainsKey(window))
        {
            resizer = default;
            return false;
        }
        else
        {
            resizer = s_Attached[window];
            return true;
        }
    }

    /// <summary>
    /// Occurs when the dock of the <see cref="Window"/> changed.
    /// </summary>
    public event EventHandler<BorderlessWindowResizer, WindowDockEventArgs>? WindowDockChanged;
    /// <summary>
    /// Occurs when the <see cref="Window"/> has been normalized.
    /// </summary>
    public event EventHandler<Window>? WindowNormalized;
    /// <summary>
    /// Occurs when the <see cref="Window"/> has been maximized.
    /// </summary>
    public event EventHandler<Window>? WindowMaximized;
}