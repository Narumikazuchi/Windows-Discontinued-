namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Relays a command to an <see cref="Action{T}"/> delegate.
/// </summary>
public partial class RelayCommand<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="onExecute">The <see cref="Action{T}"/> delegate to be invoked on command execution.</param>
    public RelayCommand([DisallowNull] Action<T> onExecute) :
        this(onExecute: onExecute,
             canExecute: null)
    { }
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="onExecute">The <see cref="Action{T}"/> delegate to be invoked on command execution.</param>
    /// <param name="canExecute">The <see cref="Func{T, TResult}"/> delegate to be invoked upon checking whether the command can be executed.</param>
    public RelayCommand([DisallowNull] Action<T> onExecute,
                        [AllowNull] Func<T, Boolean>? canExecute)
    {
        ArgumentNullException.ThrowIfNull(onExecute);

        m_OnExecute = onExecute;
        m_CanExecute = canExecute;
    }

    private readonly Action<T> m_OnExecute;
    private readonly Func<T, Boolean>? m_CanExecute;
}