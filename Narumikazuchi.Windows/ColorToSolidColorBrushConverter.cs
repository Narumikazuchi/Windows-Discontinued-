using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Converts between <see cref="Color"/> and <see cref="SolidColorBrush"/>.
    /// </summary>
    public sealed class ColorToSolidColorBrushConverter : IValueConverter
    {
        #region IValueConverter

        /// <inheritdoc/>
        [Pure]
        public Object? Convert(Object value, Type targetType, Object parameter, CultureInfo culture) => 
            value is null ||
            value is not Color color
                ? null
                : new SolidColorBrush(color);

        /// <inheritdoc/>
        [Pure]
        public Object? ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) =>
            value is null ||
            value is not SolidColorBrush brush
                ? null
                : brush.Color;

        #endregion
    }
}
