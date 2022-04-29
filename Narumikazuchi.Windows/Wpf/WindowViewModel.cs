using Point = System.Windows.Point;

namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents a view model which implements commands for windows with custom window chrome.
/// </summary>
public abstract partial class WindowViewModel
{
    /// <summary>
    /// Gets the <see cref="ICommand"/> which handles the opening of the system menu.
    /// </summary>
    public ICommand OpenSystemMenuCommand =>
        m_OpenSystemMenuCommand;

    /// <summary>
    /// Gets the <see cref="ICommand"/> which handles the minimizing of the window.
    /// </summary>
    public ICommand MinimizeWindowCommand =>
        m_MinimizeWindowCommand;

    /// <summary>
    /// Gets the <see cref="ICommand"/> which handles the maximizing of the window.
    /// </summary>
    public ICommand MaximizeWindowCommand =>
        m_MaximizeWindowCommand;

    /// <summary>
    /// Gets the <see cref="ICommand"/> which handles the closing of the window.
    /// </summary>
    public ICommand CloseWindowCommand =>
        m_CloseWindowCommand;
}

// Non-Public
partial class WindowViewModel : NotifyingViewModel
{
    /// <summary>
    /// Initalizes a new instance of the <see cref="WindowViewModel"/> class.
    /// </summary>
    protected WindowViewModel()
    {
        m_OpenSystemMenuCommand = new(this.OpenSystemMenu);
        m_MinimizeWindowCommand = new(this.MinimizeWindow);
        m_MaximizeWindowCommand = new(this.MaximizeWindow);
        m_CloseWindowCommand = new(this.CloseWindow);
    }

    /// <summary>
    /// Opens the system menu for the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> in focus.</param>
    protected virtual void OpenSystemMenu([DisallowNull] Window window) =>
        OpenSystemMenuInternal(window);

    /// <summary>
    /// Minimizes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to minimize.</param>
    protected virtual void MinimizeWindow([DisallowNull] Window window) =>
        MinimizeWindowInternal(window);

    /// <summary>
    /// Maximizes or normalizes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to normalize or maximize.</param>
    protected virtual void MaximizeWindow([DisallowNull] Window window) =>
        MaximizeWindowInternal(window);

    /// <summary>
    /// Closes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to close.</param>
    protected virtual void CloseWindow([DisallowNull] Window window) =>
        CloseWindowInternal(window);

    private static void OpenSystemMenuInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.ShowSystemMenu(window: window,
                                      screenLocation: GetMouseLocation(window));
    }

    private static void MinimizeWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.MinimizeWindow(window);
    }

    private static void MaximizeWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (window.WindowState is WindowState.Normal)
        {
            SystemCommands.MaximizeWindow(window);
            return;
        }
        window.WindowState = WindowState.Normal;
    }

    private static void CloseWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.CloseWindow(window);
    }

    private static Point GetMouseLocation(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        Point temp = Mouse.GetPosition(window);
        return new(x: temp.X + window.Left,
                   y: temp.Y + window.Top);
    }

    private readonly RelayCommand<Window> m_OpenSystemMenuCommand;
    private readonly RelayCommand<Window> m_MinimizeWindowCommand;
    private readonly RelayCommand<Window> m_MaximizeWindowCommand;
    private readonly RelayCommand<Window> m_CloseWindowCommand;
}