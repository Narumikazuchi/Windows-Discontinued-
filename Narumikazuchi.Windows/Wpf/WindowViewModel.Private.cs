using Point = System.Windows.Point;

namespace Narumikazuchi.Windows.Wpf;

public partial class WindowViewModel
{
    static private void OpenSystemMenuInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.ShowSystemMenu(window: window,
                                      screenLocation: GetMouseLocation(window));
    }

    static private void MinimizeWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.MinimizeWindow(window);
    }

    static private void MaximizeWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (window.WindowState is WindowState.Normal)
        {
            SystemCommands.MaximizeWindow(window);
            return;
        }
        else
        {
            window.WindowState = WindowState.Normal;
        }
    }

    static private void CloseWindowInternal(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        SystemCommands.CloseWindow(window);
    }

    static private Point GetMouseLocation(Window window)
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