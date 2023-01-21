using Control = System.Windows.Controls.Control;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;

namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents a font.
/// </summary>
public sealed partial class Font
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Font"/> struct.
    /// </summary>
    /// <param name="family">The font family.</param>
    /// <param name="size">The font size.</param>
    /// <param name="stretch">The stretch of the font.</param>
    /// <param name="style">The font style.</param>
    /// <param name="weight">The font weight.</param>
    /// <exception cref="ArgumentNullException"/>
    public Font([DisallowNull] FontFamily family,
                Double size,
                FontStretch stretch,
                FontStyle style,
                FontWeight weight)
    {
        ArgumentNullException.ThrowIfNull(family);

        m_Family = family;
        Typeface typeface = new(family,
                                style,
                                weight,
                                stretch);
        m_Typeface = typeface;
        this.Size = size;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Font"/> struct.
    /// </summary>
    /// <param name="family">The font family.</param>
    /// <param name="size">The font size.</param>
    /// <param name="typeface">The typeface information for the font.</param>
    /// <exception cref="ArgumentNullException"/>
    public Font([DisallowNull] FontFamily family,
                Double size,
                [DisallowNull] Typeface typeface)
    {
        ArgumentNullException.ThrowIfNull(family);
        ArgumentNullException.ThrowIfNull(typeface);

        m_Family = family;
        m_Typeface = typeface;
        this.Size = size;
    }

    /// <summary>
    /// Gets the currently used <see cref="Font"/> from the <see cref="Control"/>.
    /// </summary>
    /// <param name="control">The control to get the font from.</param>
    /// <exception cref="ArgumentNullException"/>
    static public Font FromControl([DisallowNull] Control control)
    {
        ArgumentNullException.ThrowIfNull(control);

        return new(family: control.FontFamily,
                   size: control.FontSize,
                   stretch: control.FontStretch,
                   style: control.FontStyle,
                   weight: control.FontWeight);
    }

    /// <summary>
    /// Applies this font to the specified <see cref="Control"/>.
    /// </summary>
    /// <param name="control">The control to apply the font to.</param>
    /// <exception cref="ArgumentNullException"/>
    public void ApplyTo([DisallowNull] Control control)
    {
        ArgumentNullException.ThrowIfNull(control);

        control.FontFamily = this.Family;
        control.FontSize = this.Size;
        control.FontStyle = this.Style;
        control.FontStretch = this.Stretch;
        control.FontWeight = this.Weight;
    }

    /// <summary>
    /// Gets the default <see cref="Font"/> object.
    /// </summary>
    [Pure]
    public static Font Default { get; } = new(family: new(familyName: "Segoe UI"),
                                              size: 12d,
                                              typeface: new(fontFamily: new(familyName: "Segoe UI"),
                                                            style: FontStyles.Normal,
                                                            weight: FontWeights.Normal,
                                                            stretch: FontStretches.Normal));

    /// <summary>
    /// Gets the font family.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [Pure]
    [NotNull]
    public FontFamily Family
    {
        get
        {
            return m_Family;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            m_Family = value;
        }
    }

    /// <summary>
    /// Gets the font size.
    /// </summary>
    [Pure]
    public Double Size
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the typeface of the font.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [Pure]
    [NotNull]
    public Typeface Typeface
    {
        get
        {
            return m_Typeface;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            m_Typeface = value;
        }
    }

    /// <summary>
    /// Gets the font stretch.
    /// </summary>
    [Pure]
    public FontStretch Stretch
    {
        get
        {
            return this.Typeface.Stretch;
        }
    }

    /// <summary>
    /// Gets the font style.
    /// </summary>
    [Pure]
    public FontStyle Style
    {
        get
        {
            return this.Typeface.Style;
        }
    }

    /// <summary>
    /// Gets the font weight.
    /// </summary>
    [Pure]
    public FontWeight Weight
    {
        get
        {
            return this.Typeface.Weight;
        }
    }

    private readonly FontFamily m_Family;
    private readonly Typeface m_Typeface;
}