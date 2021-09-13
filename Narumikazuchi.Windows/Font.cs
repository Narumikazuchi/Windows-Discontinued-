using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents a font. 
    /// </summary>
    public readonly partial struct Font
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
            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }

            this._family = family;
            Typeface typeface = new(family, 
                                    style, 
                                    weight, 
                                    stretch);
            this._typeface = typeface;
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
            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }
            if (typeface is null)
            {
                throw new ArgumentNullException(nameof(typeface));
            }

            this._family = family;
            this._typeface = typeface;
            this.Size = size;
        }

        /// <summary>
        /// Applies this font to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The control to apply the font to.</param>
        /// <exception cref="ArgumentNullException"/>
        public void ApplyTo([DisallowNull] Control control)
        {
            if (control is null)
            {
                throw new ArgumentNullException(nameof(control));
            }

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
        public static Font FromControl([DisallowNull] Control control) =>
            control is null 
                ? throw new ArgumentNullException(nameof(control)) 
                : new(control.FontFamily, 
                      control.FontSize, 
                      control.FontStretch, 
                      control.FontStyle, 
                      control.FontWeight);

        /// <summary>
        /// Gets the default <see cref="Font"/> object (since default(Font) will result in Exceptions).
        /// </summary>
        [Pure]
        public static Font Default => new(new("Segoe UI"), 
                                          12d, 
                                          new(new("Segoe UI"), 
                                              FontStyles.Normal, 
                                              FontWeights.Normal, 
                                              FontStretches.Normal));

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        [Pure]
        [NotNull]
        public FontFamily Family
        {
            get => this._family is null 
                        ? throw new ArgumentNullException(nameof(this._family)) 
                        : this._family;
            init
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this._family = value;
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
            get => this._typeface is null 
                        ? throw new ArgumentNullException(nameof(this._typeface)) 
                        : this._typeface;
            init
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this._typeface = value;
            }
        }
        /// <summary>
        /// Gets the font stretch.
        /// </summary>
        [Pure]
        public FontStretch Stretch => this.Typeface.Stretch;
        /// <summary>
        /// Gets the font style.
        /// </summary>
        [Pure]
        public FontStyle Style => this.Typeface.Style;
        /// <summary>
        /// Gets the font weight.
        /// </summary>
        [Pure]
        public FontWeight Weight => this.Typeface.Weight;
    }

    // Non-Public
    partial struct Font
    {
        private readonly FontFamily _family;
        private readonly Typeface _typeface;
    }

    // IEquatable<Font>
    partial struct Font : IEquatable<Font>
    {
        /// <inheritdoc/>
        [Pure]
        public Boolean Equals(Font other) =>
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
        public static Boolean operator ==(Font left, Font right) =>
            left.Equals(right);
        public static Boolean operator !=(Font left, Font right) =>
            !left.Equals(right);
#pragma warning restore
    }
}
