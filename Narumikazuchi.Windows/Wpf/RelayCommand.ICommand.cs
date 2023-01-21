namespace Narumikazuchi.Windows.Wpf;

public partial class RelayCommand : ICommand
{
    Boolean ICommand.CanExecute(Object? parameter)
    {
        if (m_CanExecute is null ||
            m_CanExecute.Invoke())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ICommand.Execute(Object? parameter)
    {
        m_OnExecute.Invoke();
    }

    /// <inheritdoc/>
    public event EventHandler? CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }
}