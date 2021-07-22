using System;
using System.Diagnostics;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// The <see cref="ThemeManager{TTheme}"/> singleton loads and handles visual themes for windowed applications.
    /// </summary>
    /// <remarks>
    /// This class is an <see cref="ISingleton{T}"/> and can therefore only be accessed by <see cref="ISingleton{T}.Instance"/>.
    /// </remarks>
    [DebuggerDisplay("{SelectedTheme.Name}")]
    public sealed class ThemeManager<TTheme> : ISingleton<ThemeManager<TTheme>> where TTheme : struct, IEquatable<TTheme>, ITheme
    {
        #region Constructor

        private ThemeManager() { }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the applications visual theme has changed.
        /// </summary>
        public event EventHandler<ThemeManager<TTheme>, ThemeChangedEventArgs<TTheme>>? ThemeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently selected <typeparamref name="TTheme"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
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
                this.ThemeChanged?.Invoke(this, new(this._theme));
            }
        }

        #endregion

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TTheme _theme = default;

        #endregion
    }
}
