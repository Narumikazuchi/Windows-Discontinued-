namespace Narumikazuchi.Windows;

/// <summary>
/// Represents a color on the HSV color space.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly partial struct HsvColor
{
    /// <summary>
    /// Creates a new <see cref="HsvColor"/> from the specified values.
    /// </summary>
    /// <param name="hue">The hue-component of the color.</param>
    /// <param name="saturation">The saturation-component of the color.</param>
    /// <param name="value">The value-component of the color.</param>
    public static HsvColor FromHsv(in Double hue,
                                   in Double saturation,
                                   in Double value) =>
        new()
        {
            Hue = hue,
            Saturation = saturation,
            Value = value
        };

    /// <summary>
    /// Creates a new <see cref="HsvColor"/> from the specified values.
    /// </summary>
    /// <param name="red">The red-component of the RGB color space.</param>
    /// <param name="green">The green-component of the RGB color space.</param>
    /// <param name="blue">The blue-component of the RGB color space.</param>
    public static HsvColor FromRgb(in Byte red,
                                   in Byte green,
                                   in Byte blue)
    {
        (Double h, Double s, Double v) hsv = RgbToHsv(red: red,
                                                      green: green,
                                                      blue: blue);
        return new()
        {
            Hue = hsv.h,
            Saturation = hsv.s,
            Value = hsv.v
        };
    }

    /// <summary>
    /// Creates a new <see cref="HsvColor"/> from the specified values.
    /// </summary>
    /// <param name="hue">The hue-component of the color.</param>
    /// <param name="saturation">The saturation-component of the color.</param>
    /// <param name="light">The light-component of the color.</param>
    public static HsvColor FromHsl(in Double hue,
                                   in Double saturation,
                                   in Double light)
    {
        HslColor hsl = HslColor.FromHsl(hue, saturation, light);
        MColor rgb = (MColor)hsl;
        (Double h, Double s, Double v) hsv = RgbToHsv(red: rgb.R,
                                                      green: rgb.G,
                                                      blue: rgb.B);
        return new()
        {
            Hue = hsv.h,
            Saturation = hsv.s,
            Value = hsv.v
        };
    }

    /// <inheritdoc/>
    [Pure]
    [return: MaybeNull]
    public override String? ToString()
    {
        (Byte r, Byte g, Byte b) rgb = HsvToRgb(hue: this.Hue,
                                                saturation: this.Saturation,
                                                value: this.Value);
        MColor color = MColor.FromArgb(a: 255,
                                       r: rgb.r,
                                       g: rgb.g,
                                       b: rgb.b);
        return color.ToString();
    }

#pragma warning disable
    public static explicit operator DColor(in HsvColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HsvToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                value: color.Value);
        return DColor.FromArgb(alpha: 255,
                               red: rgb.r,
                               green: rgb.g,
                               blue: rgb.b);
    }

    public static explicit operator MColor(in HsvColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HsvToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                value: color.Value);
        return MColor.FromArgb(a: 255,
                               r: rgb.r,
                               g: rgb.g,
                               b: rgb.b);
    }

    public static explicit operator HslColor(in HsvColor color)
    {
        MColor temp = (MColor)color;
        return (HslColor)temp;
    }

    public static explicit operator HsvColor(in DColor color)
    {
        (Double h, Double s, Double v) hsv = RgbToHsv(red: color.R,
                                                      green: color.G,
                                                      blue: color.B);
        return new()
        {
            Hue = hsv.h,
            Saturation = hsv.s,
            Value = hsv.v
        };
    }

    public static explicit operator HsvColor(in MColor color)
    {
        (Double h, Double s, Double v) hsv = RgbToHsv(red: color.R,
                                                      green: color.G,
                                                      blue: color.B);
        return new()
        {
            Hue = hsv.h,
            Saturation = hsv.s,
            Value = hsv.v
        };
    }

    public static explicit operator HsvColor(in HslColor color)
    {
        MColor temp = (MColor)color;
        return (HsvColor)temp;
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
    /// Gets or sets the value-component of this color.
    /// </summary>
    [Pure]
    public Double Value { get; init; }
}

// Non-Public
partial struct HsvColor
{
    private static (Byte r, Byte g, Byte b) HsvToRgb(in Double hue,
                                                     in Double saturation,
                                                     in Double value)
    {
        if (saturation == 0)
        {
            return ((Byte)Math.Round(value * 255),
                    (Byte)Math.Round(value * 255),
                    (Byte)Math.Round(value * 255));
        }

        Double h = hue == 360 ? 0 : hue / 60;

        Int32 i = (Int32)Math.Truncate(h);
        Double f = h - i;

        Double p = value * (1.0 - saturation);
        Double q = value * (1.0 - saturation * f);
        Double t = value * (1.0 - saturation * (1.0 - f));

        return i switch
        {
            0 => ((Byte)Math.Round(value * 255),
                  (Byte)Math.Round(t * 255),
                  (Byte)Math.Round(p * 255)),
            1 => ((Byte)Math.Round(q * 255),
                  (Byte)Math.Round(value * 255),
                  (Byte)Math.Round(p * 255)),
            2 => ((Byte)Math.Round(p * 255),
                  (Byte)Math.Round(value * 255),
                  (Byte)Math.Round(t * 255)),
            3 => ((Byte)Math.Round(p * 255),
                  (Byte)Math.Round(q * 255),
                  (Byte)Math.Round(value * 255)),
            4 => ((Byte)Math.Round(t * 255),
                  (Byte)Math.Round(p * 255),
                  (Byte)Math.Round(value * 255)),
            _ => ((Byte)Math.Round(value * 255),
                  (Byte)Math.Round(p * 255),
                  (Byte)Math.Round(q * 255)),
        };
    }

    private static (Double h, Double s, Double v) RgbToHsv(in Byte red,
                                                           in Byte green,
                                                           in Byte blue)
    {
        Double min = Math.Min(Math.Min(red,
                                       green),
                              blue);
        Double v = Math.Max(Math.Max(red,
                                     green),
                            blue);
        Double delta = v - min;
        Double s = v == 0d
                    ? 0d
                    : delta / v;
        Double h = 0d;
        if (red == v)
        {
            h = (green - blue) / delta;
        }
        else if (green == v)
        {
            h = 2 + (blue - red) / delta;
        }
        else if (blue == v)
        {
            h = 4 + (red - green) / delta;
        }

        h *= 60;
        while (h < 0)
        {
            h += 360d;
        }
        while (h > 360d)
        {
            h -= 360d;
        }

        return (h, s, v / 255);
    }
}

// IEquatable<HsvColor>
partial struct HsvColor : IEquatable<HsvColor>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals(HsvColor other) =>
        this.Hue == other.Hue &&
        this.Saturation == other.Saturation &&
        this.Value == other.Value;

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj) =>
        obj is HsvColor other &&
        this.Equals(other);

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode() =>
        this.Hue.GetHashCode() ^
        this.Saturation.GetHashCode() ^
        this.Value.GetHashCode();

#pragma warning disable
    public static Boolean operator ==(in HsvColor left, in HsvColor right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(in HsvColor left, in HsvColor right)
    {
        return !left.Equals(right);
    }
#pragma warning restore
}