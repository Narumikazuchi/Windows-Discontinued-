namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Relays a command to an <see cref="Action"/> delegate.
/// </summary>
public partial class RelayCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="onExecute">The <see cref="Action"/> delegate to be invoked on command execution.</param>
    public RelayCommand([DisallowNull] Action onExecute) :
        this(onExecute: onExecute,
             canExecute: null)
    { }
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="onExecute">The <see cref="Action"/> delegate to be invoked on command execution.</param>
    /// <param name="canExecute">The <see cref="Func{TResult}"/> delegate to be invoked upon checking whether the command can be executed.</param>
    public RelayCommand([DisallowNull] Action onExecute,
                        [AllowNull] Func<Boolean>? canExecute)
    {
        ArgumentNullException.ThrowIfNull(onExecute);

        m_OnExecute = onExecute;
        m_CanExecute = canExecute;
    }

    private readonly Action m_OnExecute;
    private readonly Func<Boolean>? m_CanExecute;
}