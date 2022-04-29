using Color = System.Windows.Media.Color;

namespace Narumikazuchi.Windows.Wpf;

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
                           [DisallowNull] CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(culture);

        if (value is null ||
            value is not Color color)
        {
            return null;
        }
        else
        {
            return new SolidColorBrush(color);
        }
    }

    /// <inheritdoc/>
    [return: MaybeNull]
    public Object? ConvertBack([AllowNull] Object? value,
                               [DisallowNull] Type targetType,
                               [DisallowNull] Object parameter,
                               [DisallowNull] CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(culture);

        if (value is null ||
            value is not SolidColorBrush brush)
        {
            return null;
        }
        else
        {
            return brush.Color;
        }
    }
}