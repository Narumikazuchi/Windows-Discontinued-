using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents the spectrum slider next to the color canvas on a <see cref="ColorPicker"/> object.
/// </summary>
[TemplatePart(Name = PART_SPECTRUMDISPLAY, Type = typeof(Rectangle))]
public sealed partial class ColorSpectrumSlider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpectrumSlider"/> class.
    /// </summary>
    public ColorSpectrumSlider()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        m_SpectrumDisplay = this.GetTemplateChild<Rectangle>(PART_SPECTRUMDISPLAY);
        this.CreateSpectrum();
        this.OnValueChanged(oldValue: Double.NaN,
                            newValue: this.Value);
    }

    /// <summary>
    /// Identifies the <see cref="SelectedColor"/> property.
    /// </summary>
    [NotNull]
    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register(name: nameof(SelectedColor),
                                    propertyType: typeof(Color),
                                    ownerType: typeof(ColorSpectrumSlider),
                                    typeMetadata: new FrameworkPropertyMetadata(Colors.White));

    /// <summary>
    /// Gets or sets the currently selected <see cref="Color"/>.
    /// </summary>
    public Color SelectedColor
    {
        get
        {
            return (Color)this.GetValue(SelectedColorProperty);
        }
        set
        {
            this.SetValue(dp: SelectedColorProperty,
                                     value: value);
        }
    }
}