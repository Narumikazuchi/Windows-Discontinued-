namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents a view model which implements commands for windows with custom window chrome.
/// </summary>
public abstract partial class WindowViewModel : NotifyingViewModel
{
    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> which handles the opening of the system menu.
    /// </summary>
    public RelayCommand<Window> OpenSystemMenuCommand
    {
        get
        {
            return m_OpenSystemMenuCommand;
        }
    }

    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> which handles the minimizing of the window.
    /// </summary>
    public RelayCommand<Window> MinimizeWindowCommand
    {
        get
        {
            return m_MinimizeWindowCommand;
        }
    }

    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> which handles the maximizing of the window.
    /// </summary>
    public RelayCommand<Window> MaximizeWindowCommand
    {
        get
        {
            return m_MaximizeWindowCommand;
        }
    }

    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> which handles the closing of the window.
    /// </summary>
    public RelayCommand<Window> CloseWindowCommand
    {
        get
        {
            return m_CloseWindowCommand;
        }
    }
}