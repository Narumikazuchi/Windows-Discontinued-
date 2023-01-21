using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using TextBox = System.Windows.Controls.TextBox;

namespace Narumikazuchi.Windows.Wpf;

/// <summary>
/// Represents a window which can select a <see cref="Color"/>.
/// </summary>
public sealed partial class ColorPicker : Window
{
    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner)
    {
        return Show(owner: owner,
                    allowAlhpaChannel: true);
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner,
                                     Boolean allowAlhpaChannel)
    {
        ArgumentNullException.ThrowIfNull(owner);

        ColorPicker picker = new(owner)
        {
            SelectedColor = Color.FromArgb(255,
                                           0,
                                           0,
                                           0),
            AllowAlpha = allowAlhpaChannel
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
        {
            return picker.SelectedColor;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <param name="withStyle">The style to apply to the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner,
                                     Boolean allowAlhpaChannel,
                                     [DisallowNull] Style withStyle)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(withStyle);

        ColorPicker picker = new(owner)
        {
            SelectedColor = Color.FromArgb(255,
                                           0,
                                           0,
                                           0),
            AllowAlpha = allowAlhpaChannel,
            Style = withStyle
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
        {
            return picker.SelectedColor;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="initial">The initial <see cref="Color"/> selected by the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner,
                                     Color initial)
    {
        return Show(owner: owner,
                    initial: initial,
                    allowAlhpaChannel: true);
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <param name="initial">The initial <see cref="Color"/> selected by the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner,
                                     Color initial,
                                     Boolean allowAlhpaChannel)
    {
        ArgumentNullException.ThrowIfNull(owner);

        ColorPicker picker = new(owner)
        {
            SelectedColor = initial,
            AllowAlpha = allowAlhpaChannel
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
        {
            return picker.SelectedColor;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <param name="initial">The initial <see cref="Color"/> selected by the <see cref="ColorPicker"/>.</param>
    /// <param name="withStyle">The style to apply to the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    static public Option<Color> Show([DisallowNull] Window owner,
                                     Color initial,
                                     Boolean allowAlhpaChannel,
                                     [DisallowNull] Style withStyle)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(withStyle);

        ColorPicker picker = new(owner)
        {
            SelectedColor = initial,
            AllowAlpha = allowAlhpaChannel,
            Style = withStyle
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
        {
            return picker.SelectedColor;
        }
        else
        {
            return default;
        }
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (m_ColorShadingCanvas is not null)
        {
            m_ColorShadingCanvas.MouseLeftButtonDown -= this.ColorShadingCanvas_MouseLeftButtonDown;
            m_ColorShadingCanvas.MouseLeftButtonUp -= this.ColorShadingCanvas_MouseLeftButtonUp;
            m_ColorShadingCanvas.MouseMove -= this.ColorShadingCanvas_MouseMove;
            m_ColorShadingCanvas.SizeChanged -= this.ColorShadingCanvas_SizeChanged;
        }

        m_ColorShadingCanvas = this.GetTemplateChild<Canvas>(nameof(m_ColorShadingCanvas));

        if (m_ColorShadingCanvas is not null)
        {
            m_ColorShadingCanvas.MouseLeftButtonDown += this.ColorShadingCanvas_MouseLeftButtonDown;
            m_ColorShadingCanvas.MouseLeftButtonUp += this.ColorShadingCanvas_MouseLeftButtonUp;
            m_ColorShadingCanvas.MouseMove += this.ColorShadingCanvas_MouseMove;
            m_ColorShadingCanvas.SizeChanged += this.ColorShadingCanvas_SizeChanged;
        }

        m_ColorShadeSelector = this.GetTemplateChild<Canvas>(nameof(m_ColorShadeSelector));

        if (m_ColorShadeSelector is not null)
        {
            m_ColorShadeSelector.RenderTransform = m_ColorShadeSelectorTransform;
        }

        if (m_SpectrumSlider is not null)
        {
            m_SpectrumSlider.ValueChanged -= this.SpectrumSlider_ValueChanged;
        }

        m_SpectrumSlider = this.GetTemplateChild<ColorSpectrumSlider>(nameof(m_SpectrumSlider));

        if (m_SpectrumSlider is not null)
        {
            m_SpectrumSlider.ValueChanged += this.SpectrumSlider_ValueChanged;
        }

        if (m_HexadecimalText is not null)
        {
            m_HexadecimalText.LostFocus -= this.HexadecimalText_LostFocus;
        }

        m_HexadecimalText = this.GetTemplateChild<TextBox>(nameof(m_HexadecimalText));

        if (m_HexadecimalText is not null)
        {
            m_HexadecimalText.LostFocus += this.HexadecimalText_LostFocus;
        }

        if (m_RSlider is not null)
        {
            m_RSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
        }

        m_RSlider = this.GetTemplateChild<Slider>(nameof(m_RSlider));

        if (m_RSlider is not null)
        {
            m_RSlider.ValueChanged += this.RgbaSlider_ValueChanged;
        }

        if (m_GSlider is not null)
        {
            m_GSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
        }

        m_GSlider = this.GetTemplateChild<Slider>(nameof(m_GSlider));

        if (m_GSlider is not null)
        {
            m_GSlider.ValueChanged += this.RgbaSlider_ValueChanged;
        }

        if (m_BSlider is not null)
        {
            m_BSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
        }

        m_BSlider = this.GetTemplateChild<Slider>(nameof(m_BSlider));

        if (m_BSlider is not null)
        {
            m_BSlider.ValueChanged += this.RgbaSlider_ValueChanged;
        }

        if (m_ASlider is not null)
        {
            m_ASlider.ValueChanged -= this.RgbaSlider_ValueChanged;
        }

        m_ASlider = this.GetTemplateChild<Slider>(nameof(m_ASlider));

        if (m_ASlider is not null)
        {
            m_ASlider.ValueChanged += this.RgbaSlider_ValueChanged;
            m_ASlider.Visibility = m_AllowAlpha ? Visibility.Visible : Visibility.Collapsed;
        }

        if (m_BtnOk is not null)
        {
            m_BtnOk.Click -= this.Ok_Click;
        }

        m_BtnOk = this.GetTemplateChild<Button>(nameof(m_BtnOk));
        m_BtnOk.Click += this.Ok_Click;

        this.UpdateColorShadeSelectorPosition(this.SelectedColor);
    }

    /// <summary>
    /// Identifies the <see cref="SelectedColor"/> property.
    /// </summary>
    [NotNull]
    static public readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register(name: nameof(SelectedColor),
                                    propertyType: typeof(Color),
                                    ownerType: typeof(ColorPicker),
                                    typeMetadata: new FrameworkPropertyMetadata(default(Color)));

    /// <summary>
    /// Gets or sets the <see cref="Color"/> that this picker has currently selected.
    /// </summary>
    public Color SelectedColor
    {
        get
        {
            return (Color)this.GetValue(SelectedColorProperty);
        }
        set
        {
            this.SetValue(SelectedColorProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets if the <see cref="Color.A"/> alpha values should be editable.
    /// </summary>
    public Boolean AllowAlpha
    {
        get
        {
            return m_AllowAlpha &&
                   m_ASlider is not null &&
                   m_ASlider.Visibility == Visibility.Visible;
        }
        set
        {
            m_AllowAlpha = value;
            if (m_ASlider is null)
            {
                return;
            }

            if (value)
            {
                m_ASlider.Visibility = Visibility.Visible;
            }
            else
            {
                m_ASlider.Visibility = Visibility.Collapsed;
            }
        }
    }
}