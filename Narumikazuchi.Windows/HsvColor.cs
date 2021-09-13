using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents a color on the HSV color space.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public readonly partial struct HsvColor
    {
        /// <summary>
        /// Creates a new <see cref="HsvColor"/> from the specified values.
        /// </summary>
        /// <param name="h">The hue-component of the color.</param>
        /// <param name="s">The saturation-component of the color.</param>
        /// <param name="v">The value-component of the color.</param>
        public static HsvColor FromHsv(in Double h, 
                                       in Double s, 
                                       in Double v) => 
            new() 
            { 
                H = h, 
                S = s, 
                V = v 
            };
        /// <summary>
        /// Creates a new <see cref="HsvColor"/> from the specified values.
        /// </summary>
        /// <param name="r">The red-component of the RGB color space.</param>
        /// <param name="g">The green-component of the RGB color space.</param>
        /// <param name="b">The blue-component of the RGB color space.</param>
        public static HsvColor FromRgb(in Byte r, 
                                       in Byte g, 
                                       in Byte b)
        {
            (Double, Double, Double) hsv = RgbToHsv(r, 
                                                    g, 
                                                    b);
            return new() 
            { 
                H = hsv.Item1, 
                S = hsv.Item2, 
                V = hsv.Item3 
            };
        }
        /// <summary>
        /// Creates a new <see cref="HsvColor"/> from the specified values.
        /// </summary>
        /// <param name="h">The hue-component of the color.</param>
        /// <param name="s">The saturation-component of the color.</param>
        /// <param name="l">The light-component of the color.</param>
        public static HsvColor FromHsl(in Double h, 
                                       in Double s, 
                                       in Double l)
        {
            HslColor hsl = HslColor.FromHsl(h, 
                                            s, 
                                            l);
            MColor rgb = (MColor)hsl;
            (Double, Double, Double) hsv = RgbToHsv(rgb.R, 
                                                    rgb.G, 
                                                    rgb.B);
            return new() 
            { 
                H = hsv.Item1, 
                S = hsv.Item2, 
                V = hsv.Item3 
            };
        }

        /// <inheritdoc/>
        [Pure]
        [return: MaybeNull]
        public override String? ToString()
        {
            (Byte r, Byte g, Byte b) rgb = HsvToRgb(this.H, 
                                                    this.S, 
                                                    this.V);
            MColor color = MColor.FromArgb(255, 
                                           rgb.r, 
                                           rgb.g, 
                                           rgb.b);
            return color.ToString();
        }

#pragma warning disable
        public static explicit operator DColor(in HsvColor color)
        {
            (Byte r, Byte g, Byte b) rgb = HsvToRgb(color.H, 
                                                    color.S, 
                                                    color.V);
            return DColor.FromArgb(255, 
                                   rgb.r, 
                                   rgb.g, 
                                   rgb.b);
        }
        public static explicit operator MColor(in HsvColor color)
        {
            (Byte r, Byte g, Byte b) rgb = HsvToRgb(color.H, 
                                                    color.S, 
                                                    color.V);
            return MColor.FromArgb(255, 
                                   rgb.r, 
                                   rgb.g, 
                                   rgb.b);
        }
        public static explicit operator HslColor(in HsvColor color)
        {
            MColor temp = (MColor)color;
            return (HslColor)temp;
        }
        public static explicit operator HsvColor(in DColor color)
        {
            (Double h, Double s, Double v) hsv = RgbToHsv(color.R, 
                                                          color.G, 
                                                          color.B);
            return new() 
            { 
                H = hsv.h, 
                S = hsv.s, 
                V = hsv.v 
            };
        }
        public static explicit operator HsvColor(in MColor color)
        {
            (Double h, Double s, Double v) hsv = RgbToHsv(color.R, 
                                                          color.G, 
                                                          color.B);
            return new() 
            { 
                H = hsv.h, 
                S = hsv.s, 
                V = hsv.v 
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
        public Double H { get; init; }
        /// <summary>
        /// Gets or sets the saturation-component of this color.
        /// </summary>
        [Pure]
        public Double S { get; init; }
        /// <summary>
        /// Gets or sets the value-component of this color.
        /// </summary>
        [Pure]
        public Double V { get; init; }
    }

    // Non-Public
    partial struct HsvColor
    {
        private static (Byte r, Byte g, Byte b) HsvToRgb(Double h, 
                                                         in Double s, 
                                                         in Double v)
        {
            if (s == 0)
            {
                return ((Byte)Math.Round(v * 255), 
                        (Byte)Math.Round(v * 255), 
                        (Byte)Math.Round(v * 255));
            }

            h = h == 360 ? 0 : h / 60;

            Int32 i = (Int32)Math.Truncate(h);
            Double f = h - i;

            Double p = v * (1.0 - s);
            Double q = v * (1.0 - (s * f));
            Double t = v * (1.0 - (s * (1.0 - f)));

            return i switch
            {
                0 => ((Byte)Math.Round(v * 255), 
                      (Byte)Math.Round(t * 255), 
                      (Byte)Math.Round(p * 255)),
                1 => ((Byte)Math.Round(q * 255), 
                      (Byte)Math.Round(v * 255), 
                      (Byte)Math.Round(p * 255)),
                2 => ((Byte)Math.Round(p * 255), 
                      (Byte)Math.Round(v * 255), 
                      (Byte)Math.Round(t * 255)),
                3 => ((Byte)Math.Round(p * 255), 
                      (Byte)Math.Round(q * 255), 
                      (Byte)Math.Round(v * 255)),
                4 => ((Byte)Math.Round(t * 255), 
                      (Byte)Math.Round(p * 255), 
                      (Byte)Math.Round(v * 255)),
                _ => ((Byte)Math.Round(v * 255), 
                      (Byte)Math.Round(p * 255), 
                      (Byte)Math.Round(q * 255)),
            };
        }

        private static (Double h, Double s, Double v) RgbToHsv(in Byte r, 
                                                               in Byte g, 
                                                               in Byte b)
        {
            Double min = Math.Min(Math.Min(r, 
                                           g), 
                                  b);
            Double v = Math.Max(Math.Max(r, 
                                         g), 
                                b);
            Double delta = v - min;
            Double s = v == 0d 
                        ? 0d 
                        : delta / v;
            Double h = 0d;
            if (r == v)
            {
                h = (g - b) / delta;
            }
            else if (g == v)
            {
                h = 2 + (b - r) / delta;
            }
            else if (b == v)
            {
                h = 4 + (r - g) / delta;
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
            this.H == other.H && 
            this.S == other.S && 
            this.V == other.V;

        /// <inheritdoc/>
        [Pure]
        public override Boolean Equals([AllowNull] Object? obj) => 
            obj is HsvColor other && 
            this.Equals(other);

        /// <inheritdoc/>
        [Pure]
        public override Int32 GetHashCode() => 
            this.H.GetHashCode() ^ 
            this.S.GetHashCode() ^ 
            this.V.GetHashCode();

#pragma warning disable
        public static Boolean operator ==(in HsvColor left, in HsvColor right) => 
            left.Equals(right);
        public static Boolean operator !=(in HsvColor left, in HsvColor right) => 
            !left.Equals(right);
#pragma warning restore
    }
}
