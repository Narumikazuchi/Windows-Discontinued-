using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents a font. 
    /// </summary>
    public readonly struct Font
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Font"/> struct.
        /// </summary>
        /// <param name="family">The font family.</param>
        /// <param name="size">The font size.</param>
        /// <param name="stretch">The stretch of the font.</param>
        /// <param name="style">The font style.</param>
        /// <param name="weight">The font weight.</param>
        /// <exception cref="ArgumentNullException"/>
        public Font([DisallowNull] FontFamily family, in Double size, in FontStretch stretch, in FontStyle style, in FontWeight weight)
        {
            if (family is null)
            {
                throw new ArgumentNullException(nameof(family));
            }

            this._family = family;
            Typeface typeface = new(family, style, weight, stretch);
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
        public Font([DisallowNull] FontFamily family, in Double size, [DisallowNull] Typeface typeface)
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

        #endregion

        #region Apply

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
                : new(control.FontFamily, control.FontSize, control.FontStretch, control.FontStyle, control.FontWeight);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the default <see cref="Font"/> object (since default(Font) will result in Exceptions).
        /// </summary>
        public static Font Default => new(new("Segoe UI"), 12d, new(new("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal));

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public FontFamily Family
        {
            get => this._family is null ? throw new ArgumentNullException(nameof(this._family)) : this._family;
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
        public Double Size { get; init; }
        /// <summary>
        /// Gets the typeface of the font.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public Typeface Typeface
        {
            get => this._typeface is null ? throw new ArgumentNullException(nameof(this._typeface)) : this._typeface;
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
        public FontStretch Stretch => this.Typeface.Stretch;
        /// <summary>
        /// Gets the font style.
        /// </summary>
        public FontStyle Style => this.Typeface.Style;
        /// <summary>
        /// Gets the font weight.
        /// </summary>
        public FontWeight Weight => this.Typeface.Weight;

        #endregion

        #region Fields

        private readonly FontFamily _family;
        private readonly Typeface _typeface;

        #endregion
    }
}
