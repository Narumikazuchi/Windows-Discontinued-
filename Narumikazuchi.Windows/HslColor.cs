namespace Narumikazuchi.Windows;

/// <summary>
/// Represents a color on the HSL color space.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly partial struct HslColor : IEquatable<HslColor>
{
    /// <summary>
    /// Creates a new <see cref="HslColor"/> from the specified values.
    /// </summary>
    /// <param name="hue">The hue-component of the color.</param>
    /// <param name="saturation">The saturation-component of the color.</param>
    /// <param name="light">The light-component of the color.</param>
    public static HslColor FromHsl(in Double hue,
                                   in Double saturation,
                                   in Double light) =>
        new()
        {
            Hue = hue,
            Saturation = saturation,
            Light = light
        };

    /// <summary>
    /// Creates a new <see cref="HslColor"/> from the specified values.
    /// </summary>
    /// <param name="red">The red-component of the RGB color space.</param>
    /// <param name="green">The green-component of the RGB color space.</param>
    /// <param name="blue">The blue-component of the RGB color space.</param>
    public static HslColor FromRgb(in Byte red,
                                   in Byte green,
                                   in Byte blue)
    {
        (Double h, Double s, Double l) hsl = RgbToHsl(red: red,
                                                      green: green,
                                                      blue: blue);
        return new()
        {
            Hue = hsl.h,
            Saturation = hsl.s,
            Light = hsl.l
        };
    }

    /// <summary>
    /// Creates a new <see cref="HslColor"/> from the specified values.
    /// </summary>
    /// <param name="hue">The hue-component of the color.</param>
    /// <param name="saturation">The saturation-component of the color.</param>
    /// <param name="value">The value-component of the color.</param>
    public static HslColor FromHsv(in Double hue,
                                   in Double saturation,
                                   in Double value)
    {
        HsvColor hsv = HsvColor.FromHsv(hue: hue,
                                        saturation: saturation,
                                        value: value);
        MColor rgb = (MColor)hsv;
        (Double h, Double s, Double l) hsl = RgbToHsl(red: rgb.R,
                                                      green: rgb.G,
                                                      blue: rgb.B);
        return new()
        {
            Hue = hsl.h,
            Saturation = hsl.s,
            Light = hsl.l
        };
    }

    /// <inheritdoc/>
    [Pure]
    [return: MaybeNull]
    public override String? ToString()
    {
        (Byte r, Byte g, Byte b) rgb = HslToRgb(hue: this.Hue,
                                                saturation: this.Saturation,
                                                light: this.Light);
        MColor color = MColor.FromArgb(a: 255,
                                       r: rgb.r,
                                       g: rgb.g,
                                       b: rgb.b);
        return color.ToString();
    }

#pragma warning disable
    public static explicit operator DColor(in HslColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HslToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                light: color.Light);
        return DColor.FromArgb(alpha: 255,
                               red: rgb.r,
                               green: rgb.g,
                               blue: rgb.b);
    }

    public static explicit operator MColor(in HslColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HslToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                light: color.Light);
        return MColor.FromArgb(a: 255,
                               r: rgb.r,
                               g: rgb.g,
                               b: rgb.b);
    }

    public static explicit operator HsvColor(in HslColor color)
    {
        MColor temp = (MColor)color;
        return (HsvColor)temp;
    }

    public static explicit operator HslColor(in DColor color)
    {
        (Double h, Double s, Double l) hsl = RgbToHsl(red: color.R,
                                                      green: color.G,
                                                      blue: color.B);
        return new()
        {
            Hue = hsl.h,
            Saturation = hsl.s,
            Light = hsl.l
        };
    }

    public static explicit operator HslColor(in MColor color)
    {
        (Double h, Double s, Double l) hsl = RgbToHsl(red: color.R,
                                                      green: color.G,
                                                      blue: color.B);
        return new()
        {
            Hue = hsl.h,
            Saturation = hsl.s,
            Light = hsl.l
        };
    }

    public static explicit operator HslColor(in HsvColor color)
    {
        MColor temp = (MColor)color;
        return (HslColor)temp;
    }

#pragma warning restore

    /// <summary>
    /// Gets or sets the hue-component of this color.
    /// </summary>
    [Pure]
    public Double Hue { get; init; }

    /// <summary>
    /// Gets or sets the saturation-component of this color.
    /// </summary>
    [Pure]
    public Double Saturation { get; init; }

    /// <summary>
    /// Gets or sets the light-component of this color.
    /// </summary>
    [Pure]
    public Double Light { get; init; }
}

// Non-Public
partial struct HslColor
{
    private static (Byte r, Byte g, Byte b) HslToRgb(in Double hue,
                                                     in Double saturation,
                                                     in Double light)
    {
        Double c = (1 - Math.Abs(2 * light - 1)) * saturation;
        Double x = c * (1 - Math.Abs(hue / 60d % 2 - 1));
        Double m = light - c / 2;

        Double r2 = 0d;
        Double g2 = 0d;
        Double b2 = 0d;
        if (hue is >= 0d
                and < 60d)
        {
            r2 = c;
            g2 = x;
        }
        else if (hue is >= 60d
                     and < 120d)
        {
            r2 = x;
            g2 = c;
        }
        else if (hue is >= 120d
                     and < 180d)
        {
            g2 = c;
            b2 = x;
        }
        else if (hue is >= 180d
                     and < 240d)
        {
            g2 = x;
            b2 = c;
        }
        else if (hue is >= 240d
                     and < 300d)
        {
            r2 = x;
            b2 = c;
        }
        else if (hue is >= 300d
                     and < 360d)
        {
            r2 = c;
            b2 = x;
        }

        Byte r = Convert.ToByte((r2 + m) * 255);
        Byte g = Convert.ToByte((g2 + m) * 255);
        Byte b = Convert.ToByte((b2 + m) * 255);

        return (r, g, b);
    }

    private static (Double h, Double s, Double l) RgbToHsl(in Byte red,
                                                           in Byte green,
                                                           in Byte blue)
    {
        Double r2 = red / 255d;
        Double g2 = green / 255d;
        Double b2 = blue / 255d;
        Double min = Math.Min(Math.Min(r2, g2), b2);
        Double max = Math.Max(Math.Max(r2, g2), b2);
        Double c = max - min;

        Double h = 0d;
        Double l = (max + min) / 2;
        Double s = c == 0d ? 0d : c / (1 - Math.Abs(2 * l - 1));

        if (c == 0d)
        {
            h = 0d;
        }
        else if (max == r2)
        {
            Double segment = (g2 - b2) / c;
            Double shift = 0d / 60d;
            if (segment < 0)
            {
                shift = 360d / 60d;
            }
            h = segment + shift;
        }
        else if (max == g2)
        {
            Double segment = (b2 - r2) / c;
            Double shift = 120d / 60d;
            h = segment + shift;
        }
        else if (max == b2)
        {
            Double segment = (r2 - g2) / c;
            Double shift = 240d / 60d;
            h = segment + shift;
        }
        h *= 60;

        return (h, s, l);
    }
}

// IEquatable<HslColor>
partial struct HslColor : IEquatable<HslColor>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals(HslColor other) =>
        this.Hue == other.Hue &&
        this.Saturation == other.Saturation &&
        this.Light == other.Light;

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj) =>
        obj is HslColor other &&
        this.Equals(other);

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode() =>
        this.Hue.GetHashCode() ^
        this.Saturation.GetHashCode() ^
        this.Light.GetHashCode();

#pragma warning disable
    public static Boolean operator ==(in HslColor left, in HslColor right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(in HslColor left, in HslColor right)
    {
        return !left.Equals(right);
    }
#pragma warning restore
}