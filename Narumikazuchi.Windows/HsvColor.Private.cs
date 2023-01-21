namespace Narumikazuchi.Windows;

public partial struct HsvColor
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