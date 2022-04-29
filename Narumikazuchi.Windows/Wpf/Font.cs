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
                in Double size,
                in FontStretch stretch,
                in FontStyle style,
                in FontWeight weight)
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
                in Double size,
                [DisallowNull] Typeface typeface)
    {
        ArgumentNullException.ThrowIfNull(family);
        ArgumentNullException.ThrowIfNull(typeface);

        m_Family = family;
        m_Typeface = typeface;
        this.Size = size;
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
    /// Gets the currently used <see cref="Font"/> from the <see cref="Control"/>.
    /// </summary>
    /// <param name="control">The control to get the font from.</param>
    /// <exception cref="ArgumentNullException"/>
    public static Font FromControl([DisallowNull] Control control)
    {
        ArgumentNullException.ThrowIfNull(control);

        return new(family: control.FontFamily,
                   size: control.FontSize,
                   stretch: control.FontStretch,
                   style: control.FontStyle,
                   weight: control.FontWeight);
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
        get => m_Family;
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
    public Double Size { get; init; }

    /// <summary>
    /// Gets the typeface of the font.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [Pure]
    [NotNull]
    public Typeface Typeface
    {
        get => m_Typeface;
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
    public FontStretch Stretch => 
        this.Typeface.Stretch;

    /// <summary>
    /// Gets the font style.
    /// </summary>
    [Pure]
    public FontStyle Style => 
        this.Typeface.Style;

    /// <summary>
    /// Gets the font weight.
    /// </summary>
    [Pure]
    public FontWeight Weight => 
        this.Typeface.Weight;
}

// Non-Public
partial class Font
{
    private readonly FontFamily m_Family;
    private readonly Typeface m_Typeface;
}

// IEquatable<Font>
partial class Font : IEquatable<Font>
{
    /// <inheritdoc/>
    [Pure]
    public Boolean Equals([AllowNull] Font? other) =>
        other is not null &&
        this.Size == other.Size &&
        this.Stretch == other.Stretch &&
        this.Style == other.Style &&
        this.Weight == other.Weight &&
        this.Family.BaseUri == other.Family.BaseUri;

    /// <inheritdoc/>
    [Pure]
    public override Boolean Equals([AllowNull] Object? obj) =>
        obj is Font other &&
        this.Equals(other);

    /// <inheritdoc/>
    [Pure]
    public override Int32 GetHashCode() =>
        this.Size.GetHashCode() ^
        this.Stretch.GetHashCode() ^
        this.Style.GetHashCode() ^
        this.Weight.GetHashCode() ^
        this.Family.BaseUri.GetHashCode();

#pragma warning disable
    public static Boolean operator ==(Font? left, Font? right)
    {
        if (left is null)
        {
            return right is null;
        }
        else
        {
            return left.Equals(right);
        }
    }

    public static Boolean operator !=(Font? left, Font? right)
    {
        if (left is null)
        {
            return right is not null;
        }
        else
        {
            return !left.Equals(right);
        }
    }
#pragma warning restore
}