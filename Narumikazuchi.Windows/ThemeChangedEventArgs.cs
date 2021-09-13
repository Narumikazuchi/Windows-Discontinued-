using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Contains the <typeparamref name="TTheme"/> after a change occured at the <see cref="ThemeManager{TTheme}"/>.
    /// </summary>
    public sealed class ThemeChangedEventArgs<TTheme> : EventArgs where TTheme : struct, IEquatable<TTheme>, ITheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeChangedEventArgs{TTheme}"/> class.
        /// </summary>
        public ThemeChangedEventArgs(TTheme theme) => 
            this.NewTheme = theme;

        /// <summary>
        /// Gets the <see cref="ThemeChangedEventArgs{TTheme}"/> for the current theme.
        /// </summary>
        [Pure]
        [NotNull]
        public static ThemeChangedEventArgs<TTheme> Current => 
            new(Singleton<ThemeManager<TTheme>>.Instance.SelectedTheme);

        /// <summary>
        /// Gets the new <see cref="ThemeManager{TTheme}.SelectedTheme"/>.
        /// </summary>
        [Pure]
        public TTheme NewTheme { get; }
    }
}
