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
    static public HslColor FromHsl(Double hue,
                                   Double saturation,
                                   Double light)
    {
        return new()
        {
            Hue = hue,
            Saturation = saturation,
            Light = light
        };
    }

    /// <summary>
    /// Creates a new <see cref="HslColor"/> from the specified values.
    /// </summary>
    /// <param name="red">The red-component of the RGB color space.</param>
    /// <param name="green">The green-component of the RGB color space.</param>
    /// <param name="blue">The blue-component of the RGB color space.</param>
    static public HslColor FromRgb(Byte red,
                                   Byte green,
                                   Byte blue)
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
    static public HslColor FromHsv(Double hue,
                                   Double saturation,
                                   Double value)
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
    static public explicit operator DColor(HslColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HslToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                light: color.Light);
        return DColor.FromArgb(alpha: 255,
                               red: rgb.r,
                               green: rgb.g,
                               blue: rgb.b);
    }

    static public explicit operator MColor(HslColor color)
    {
        (Byte r, Byte g, Byte b) rgb = HslToRgb(hue: color.Hue,
                                                saturation: color.Saturation,
                                                light: color.Light);
        return MColor.FromArgb(a: 255,
                               r: rgb.r,
                               g: rgb.g,
                               b: rgb.b);
    }

    static public explicit operator HsvColor(HslColor color)
    {
        MColor temp = (MColor)color;
        return (HsvColor)temp;
    }

    static public explicit operator HslColor(DColor color)
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

    static public explicit operator HslColor(MColor color)
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

    static public explicit operator HslColor(HsvColor color)
    {
        MColor temp = (MColor)color;
        return (HslColor)temp;
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
    /// Gets or sets the light-component of this color.
    /// </summary>
    [Pure]
    public Double Light
    {
        get;
        init;
    }
}