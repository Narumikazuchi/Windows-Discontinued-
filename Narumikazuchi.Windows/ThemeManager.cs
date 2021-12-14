namespace Narumikazuchi.Windows;

/// <summary>
/// The <see cref="ThemeManager{TTheme}"/> singleton loads and handles visual themes for windowed applications.
/// </summary>
/// <remarks>
/// This class is an <see cref="Singleton"/> and can therefore only be accessed by <see cref="Singleton{T}.Instance"/>.
/// </remarks>
[DebuggerDisplay("{SelectedTheme.Name}")]
public sealed partial class ThemeManager<TTheme> : Singleton
    where TTheme : struct, IEquatable<TTheme>, ITheme
{
    /// <summary>
    /// Gets or sets the currently selected <typeparamref name="TTheme"/>.
    /// </summary>
    [Pure]
    public TTheme SelectedTheme
    {
        get => this._theme;
        set
        {
            if (value.Equals(default) ||
                value.Equals(this._theme))
            {
                return;
            }
            this._theme = value;
            this.ThemeChanged?.Invoke(this,
                                      new(this._theme));
        }
    }

    /// <summary>
    /// Occurs after the applications visual theme has changed.
    /// </summary>
    public event EventHandler<ThemeManager<TTheme>, ThemeChangedEventArgs<TTheme>>? ThemeChanged;
}

// Non-Public
partial class ThemeManager<TTheme>
{
    private ThemeManager()
    {
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TTheme _theme = default;
}