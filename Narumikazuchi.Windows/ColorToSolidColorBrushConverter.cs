using System;
using System.Diagnostics.CodeAnalysis;
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
        /// <inheritdoc/>
        [return: MaybeNull]
        public Object? Convert([AllowNull] Object? value, 
                               [DisallowNull] Type targetType,
                               [DisallowNull] Object parameter,
                               [DisallowNull] CultureInfo culture) => 
            value is null ||
            value is not Color color
                ? null
                : new SolidColorBrush(color);

        /// <inheritdoc/>
        [return: MaybeNull]
        public Object? ConvertBack([AllowNull] Object? value,
                                   [DisallowNull] Type targetType,
                                   [DisallowNull] Object parameter,
                                   [DisallowNull] CultureInfo culture) =>
            value is null ||
            value is not SolidColorBrush brush
                ? null
                : brush.Color;
    }
}
