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
    static public HsvColor FromHsv(Double hue,
                                   Double saturation,
                                   Double value)
    {
        return new()
        {
            Hue = hue,
            Saturation = saturation,
            Value = value
        };
    }

    /// <summary>
    /// Creates a new <see cref="HsvColor"/> from the specified values.
    /// </summary>
    /// <param name="red">The red-component of the RGB color space.</param>
    /// <param name="green">The green-component of the RGB color space.</param>
    /// <param name="blue">The blue-component of the RGB color space.</param>
    static public HsvColor FromRgb(Byte red,
                                   Byte green,
                                   Byte blue)
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
    static public HsvColor FromHsl(Double hue,
                                   Double saturation,
                                   Double light)
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
    static public explicit operator DColor(HsvColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HsvToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                value: color.Value);
        return DColor.FromArgb(alpha: 255,
                               red: rgb.r,
                               green: rgb.g,
                               blue: rgb.b);
    }

    static public explicit operator MColor(HsvColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HsvToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                value: color.Value);
        return MColor.FromArgb(a: 255,
                               r: rgb.r,
                               g: rgb.g,
                               b: rgb.b);
    }

    static public explicit operator HslColor(HsvColor color)
    {
        MColor temp = (MColor)color;
        return (HslColor)temp;
    }

    static public explicit operator HsvColor(DColor color)
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

    static public explicit operator HsvColor(MColor color)
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

    static public explicit operator HsvColor(HslColor color)
    {
        MColor temp = (MColor)color;
        return (HsvColor)temp;
    }
#pragma warning restore

    /// <summary>
    /// Gets or sets the hue-component of this color.
    /// </summary>
    [Pure]
    public Double Hue
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the saturation-component of this color.
    /// </summary>
    [Pure]
    public Double Saturation
    {
        get;
        init;
    }

    /// <summary>
    /// Gets or sets the value-component of this color.
    /// </summary>
    [Pure]
    public Double Value
    {
        get;
        init;
    }
}