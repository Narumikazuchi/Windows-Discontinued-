namespace Narumikazuchi.Windows;

public partial struct HslColor
{
    static private (Byte r, Byte g, Byte b) HslToRgb(in Double hue,
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

    static private (Double h, Double s, Double l) RgbToHsl(in Byte red,
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