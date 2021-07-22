using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents a color on the HSL color space.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public readonly struct HslColor : IEquatable<HslColor>
    {
        #region Constructor

        /// <summary>
        /// Creates a new <see cref="HslColor"/> from the specified values.
        /// </summary>
        /// <param name="h">The hue-component of the color.</param>
        /// <param name="s">The saturation-component of the color.</param>
        /// <param name="l">The light-component of the color.</param>
        public static HslColor FromHsl(in Double h, in Double s, in Double l) => new() { H = h, S = s, L = l };
        /// <summary>
        /// Creates a new <see cref="HslColor"/> from the specified values.
        /// </summary>
        /// <param name="r">The red-component of the RGB color space.</param>
        /// <param name="g">The green-component of the RGB color space.</param>
        /// <param name="b">The blue-component of the RGB color space.</param>
        public static HslColor FromRgb(in Byte r, in Byte g, in Byte b)
        {
            (Double, Double, Double) hsl = RgbToHsl(r, g, b);
            return new() { H = hsl.Item1, S = hsl.Item2, L = hsl.Item3 };
        }
        /// <summary>
        /// Creates a new <see cref="HslColor"/> from the specified values.
        /// </summary>
        /// <param name="h">The hue-component of the color.</param>
        /// <param name="s">The saturation-component of the color.</param>
        /// <param name="v">The value-component of the color.</param>
        public static HslColor FromHsv(in Double h, in Double s, in Double v)
        {
            HsvColor hsv = HsvColor.FromHsv(h, s, v);
            MColor rgb = (MColor)hsv;
            (Double, Double, Double) hsl = RgbToHsl(rgb.R, rgb.G, rgb.B);
            return new() { H = hsl.Item1, S = hsl.Item2, L = hsl.Item3 };
        }

        #endregion

        #region Conversion

        private static (Byte r, Byte g, Byte b) HslToRgb(Double h, in Double s, in Double l)
        {
            Double c = (1 - Math.Abs(2 * l - 1)) * s;
            Double x = c * (1 - Math.Abs((h / 60d % 2) - 1));
            Double m = l - (c / 2);

            Double r2 = 0d;
            Double g2 = 0d;
            Double b2 = 0d;
            if (h is >= 0d 
                  and < 60d)
            {
                r2 = c;
                g2 = x;
            }
            else if (h is >= 60d
                       and < 120d)
            {
                r2 = x;
                g2 = c;
            }
            else if (h is >= 120d
                       and < 180d)
            {
                g2 = c;
                b2 = x;
            }
            else if (h is >= 180d
                       and < 240d)
            {
                g2 = x;
                b2 = c;
            }
            else if (h is >= 240d
                       and < 300d)
            {
                r2 = x;
                b2 = c;
            }
            else if (h is >= 300d
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

        private static (Double h, Double s, Double l) RgbToHsl(in Byte r, in Byte g, in Byte b)
        {
            Double r2 = r / 255d;
            Double g2 = g / 255d;
            Double b2 = b / 255d;
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

        #endregion

        #region Object

        /// <inheritdoc/>
        [Pure]
        public override Int32 GetHashCode() => this.H.GetHashCode() ^ this.S.GetHashCode() ^ this.L.GetHashCode();

        /// <inheritdoc/>
        [Pure]
        public override Boolean Equals(Object? obj) => obj is HslColor other && this.Equals(other);

        /// <inheritdoc/>
        [Pure]
        public override String ToString()
        {
            (Byte r, Byte g, Byte b) rgb = HslToRgb(this.H, this.S, this.L);
            MColor color = MColor.FromArgb(255, rgb.r, rgb.g, rgb.b);
            return color.ToString();
        }

        #endregion

        #region IEquatable

        /// <inheritdoc/>
        [Pure]
        public Boolean Equals(HslColor other) =>
            this.H == other.H && this.S == other.S && this.L == other.L;

        #endregion

        #region Operators

#pragma warning disable
        public static explicit operator DColor(in HslColor color)
        {
            (Byte r, Byte g, Byte b) rgb = HslToRgb(color.H, color.S, color.L);
            return DColor.FromArgb(255, rgb.r, rgb.g, rgb.b);
        }
        public static explicit operator MColor(in HslColor color)
        {
            (Byte r, Byte g, Byte b) rgb = HslToRgb(color.H, color.S, color.L);
            return MColor.FromArgb(255, rgb.r, rgb.g, rgb.b);
        }
        public static explicit operator HsvColor(in HslColor color)
        {
            MColor temp = (MColor)color;
            return (HsvColor)temp;
        }
        public static explicit operator HslColor(in DColor color)
        {
            (Double h, Double s, Double l) hsl = RgbToHsl(color.R, color.G, color.B);
            return new() { H = hsl.h, S = hsl.s, L = hsl.l };
        }
        public static explicit operator HslColor(in MColor color)
        {
            (Double h, Double s, Double l) hsl = RgbToHsl(color.R, color.G, color.B);
            return new() { H = hsl.h, S = hsl.s, L = hsl.l };
        }
        public static explicit operator HslColor(in HsvColor color)
        {
            MColor temp = (MColor)color;
            return (HslColor)temp;
        }

        public static Boolean operator ==(in HslColor left, in HslColor right) => left.Equals(right);
        public static Boolean operator !=(in HslColor left, in HslColor right) => !left.Equals(right);
#pragma warning restore

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the hue-component of this color.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public Double H { get; init; }
        /// <summary>
        /// Gets or sets the saturation-component of this color.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public Double S { get; init; }
        /// <summary>
        /// Gets or sets the light-component of this color.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public Double L { get; init; }

        #endregion
    }
}
