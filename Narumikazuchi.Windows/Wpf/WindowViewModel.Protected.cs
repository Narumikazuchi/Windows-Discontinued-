namespace Narumikazuchi.Windows.Wpf;

public partial class WindowViewModel
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
    protected virtual void OpenSystemMenu([DisallowNull] Window window)
    {
        OpenSystemMenuInternal(window);
    }

    /// <summary>
    /// Minimizes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to minimize.</param>
    protected virtual void MinimizeWindow([DisallowNull] Window window)
    {
        MinimizeWindowInternal(window);
    }

    /// <summary>
    /// Maximizes or normalizes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to normalize or maximize.</param>
    protected virtual void MaximizeWindow([DisallowNull] Window window)
    {
        MaximizeWindowInternal(window);
    }

    /// <summary>
    /// Closes the <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The <see cref="Window"/> to close.</param>
    protected virtual void CloseWindow([DisallowNull] Window window)
    {
        CloseWindowInternal(window);
    }
}