using System;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Contains the <typeparamref name="TTheme"/> after a change occured at the <see cref="ThemeManager{TTheme}"/>.
    /// </summary>
    public sealed class ThemeChangedEventArgs<TTheme> : EventArgs where TTheme : struct, IEquatable<TTheme>, ITheme
    {
        #region Constructor

        internal ThemeChangedEventArgs(TTheme theme) => this.NewTheme = theme;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the new <see cref="ThemeManager{TTheme}.SelectedTheme"/>.
        /// </summary>
        public TTheme NewTheme { get; }

        #endregion
    }
}
