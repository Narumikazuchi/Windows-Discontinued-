namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Provides the needed functionality of an <see cref="ITheme"/>.
    /// </summary>
    public interface ITheme
    {
        /// <summary>
        /// Gets the color at the specified index as <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="color">The resulting <see cref="System.Drawing.Color"/>.</param>
        /// <returns><see langword="true"/> if the color was found at the specified index; otherwise, <see langword="false"/></returns>
        [System.Diagnostics.Contracts.Pure]
        public System.Boolean TryGetColor(in System.Int32 index,
                                          [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Drawing.Color? color);
        /// <summary>
        /// Gets the color at the specified index as <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="color">The resulting <see cref="System.Windows.Media.Color"/>.</param>
        /// <returns><see langword="true"/> if the color was found at the specified index; otherwise, <see langword="false"/></returns>
        [System.Diagnostics.Contracts.Pure]
        public System.Boolean TryGetColor(in System.Int32 index,
                                          [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Windows.Media.Color? color);

        /// <summary>
        /// Gets the <see cref="System.Windows.Media.Brush"/> of the color at the specified index.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="brush">The resulting <see cref="System.Windows.Media.Brush"/> of that color.</param>
        /// <returns><see langword="true"/> if the brush was found at the specified index; otherwise, <see langword="false"/></returns>
        [System.Diagnostics.Contracts.Pure]
        public System.Boolean TryGetBrush(in System.Int32 index,
                                          [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Windows.Media.Brush? brush);

        /// <summary>
        /// Gets the font at the specified index as <see cref="Font"/>.
        /// </summary>
        /// <param name="index">The index of the font to get.</param>
        /// <param name="font">The resulting <see cref="Font"/>.</param>
        /// <returns><see langword="true"/> if the font was found at the specified index; otherwise, <see langword="false"/></returns>
        [System.Diagnostics.Contracts.Pure]
        public System.Boolean TryGetFont(in System.Int32 index,
                                         [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Font? font);
        /// <summary>
        /// Gets the font at the specified index as <see cref="System.Drawing.Font"/>.
        /// </summary>
        /// <param name="index">The index of the font to get.</param>
        /// <param name="font">The resulting <see cref="System.Drawing.Font"/>.</param>
        /// <returns><see langword="true"/> if the font was found at the specified index; otherwise, <see langword="false"/></returns>
        [System.Diagnostics.Contracts.Pure]
        public System.Boolean TryGetFont(in System.Int32 index,
                                         [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Drawing.Font? font);

        /// <summary>
        /// Gets the <see cref="System.Guid"/> of this <see cref="ITheme"/>.
        /// </summary>
        [System.Diagnostics.Contracts.Pure]
        public System.Guid Guid { get; }
        /// <summary>
        /// Gets the name of this <see cref="ITheme"/>.
        /// </summary>
        [System.Diagnostics.Contracts.Pure]
        [System.Diagnostics.CodeAnalysis.NotNull]
        public System.String Name { get; }
    }
}
