namespace Narumikazuchi.Windows.Wpf;

public partial class RelayCommand<T> : ICommand
{
    Boolean ICommand.CanExecute(Object? parameter)
    {
        if (m_CanExecute is null)
        {
            return true;
        }

        if (parameter is not T myParameter)
        {
            return false;
        }

        if (m_CanExecute.Invoke(myParameter))
        {
            return true;
        }

        return false;
    }

    void ICommand.Execute(Object? parameter)
    {
        if (parameter is not T myParameter)
        {
            return;
        }

        m_OnExecute.Invoke(myParameter);
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