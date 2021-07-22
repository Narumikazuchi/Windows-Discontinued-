namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Provides the needed functionality of an <see cref="ITheme"/>.
    /// </summary>
    public interface ITheme
    {
        #region Getters

        /// <summary>
        /// Gets the color at the specified index as <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="color">The resulting <see cref="System.Drawing.Color"/>.</param>
        /// <exception cref="System.IndexOutOfRangeException" />
        public void GetColor(in System.Int32 index, out System.Drawing.Color color);
        /// <summary>
        /// Gets the color at the specified index as <see cref="System.Windows.Media.Color"/>.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="color">The resulting <see cref="System.Windows.Media.Color"/>.</param>
        /// <exception cref="System.IndexOutOfRangeException" />
        public void GetColor(in System.Int32 index, out System.Windows.Media.Color color);

        /// <summary>
        /// Gets the <see cref="System.Windows.Media.Brush"/> of the color at the specified index.
        /// </summary>
        /// <param name="index">The index of the color to get.</param>
        /// <param name="brush">The resulting <see cref="System.Windows.Media.Brush"/> of that color.</param>
        /// <exception cref="System.IndexOutOfRangeException" />
        public void GetBrush(in System.Int32 index, out System.Windows.Media.Brush brush);

        /// <summary>
        /// Gets the font at the specified index as <see cref="Font"/>.
        /// </summary>
        /// <param name="index">The index of the font to get.</param>
        /// <param name="font">The resulting <see cref="Font"/>.</param>
        /// <exception cref="System.IndexOutOfRangeException" />
        public void GetFont(in System.Int32 index, out Font font);
        /// <summary>
        /// Gets the font at the specified index as <see cref="System.Drawing.Font"/>.
        /// </summary>
        /// <param name="index">The index of the font to get.</param>
        /// <param name="font">The resulting <see cref="System.Drawing.Font"/>.</param>
        /// <exception cref="System.IndexOutOfRangeException" />
        public void GetFont(in System.Int32 index, out System.Drawing.Font font);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="System.Guid"/> of this <see cref="ITheme"/>.
        /// </summary>
        public System.Guid Guid { get; }
        /// <summary>
        /// Gets the name of this <see cref="ITheme"/>.
        /// </summary>
        public System.String Name { get; }

        #endregion
    }
}
