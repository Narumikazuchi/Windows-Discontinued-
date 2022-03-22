using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
using TextBox = System.Windows.Controls.TextBox;

namespace Narumikazuchi.Windows;

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
    public static Color? Show([DisallowNull] Window owner) =>
        Show(owner,
             true);

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    public static Color? Show([DisallowNull] Window owner,
                              in Boolean allowAlhpaChannel)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        ColorPicker picker = new(owner)
        {
            SelectedColor = Color.FromArgb(255,
                                           0,
                                           0,
                                           0),
            AllowAlpha = allowAlhpaChannel
        };
        Boolean? result = picker.ShowDialog();
        return result.HasValue &&
               result.Value
                    ? picker.SelectedColor
                    : null;
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
    public static Color? Show([DisallowNull] Window owner,
                              in Boolean allowAlhpaChannel,
                              [DisallowNull] Style withStyle)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }
        if (withStyle is null)
        {
            throw new ArgumentNullException(nameof(withStyle));
        }

        ColorPicker picker = new(owner)
        {
            SelectedColor = Color.FromArgb(255,
                                           0,
                                           0,
                                           0),
            AllowAlpha = allowAlhpaChannel,
            Style = withStyle
        };
        Boolean? result = picker.ShowDialog();
        return result.HasValue &&
               result.Value
                    ? picker.SelectedColor
                    : null;
    }

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="initial">The initial <see cref="Color"/> selected by the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    public static Color? Show([DisallowNull] Window owner,
                              in Color initial) =>
        Show(owner,
             initial,
             true);

    /// <summary>
    /// Shows the <see cref="ColorPicker"/> window and returns the <see cref="Color"/> the user selected.
    /// </summary>
    /// <param name="owner">The owning window of the <see cref="ColorPicker"/>.</param>
    /// <param name="allowAlhpaChannel">Should the alpha channel be included in the selection.</param>
    /// <param name="initial">The initial <see cref="Color"/> selected by the <see cref="ColorPicker"/>.</param>
    /// <returns>The <see cref="Color"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
    /// <exception cref="ArgumentNullException"/>
    [return: MaybeNull]
    public static Color? Show([DisallowNull] Window owner,
                              in Color initial,
                              in Boolean allowAlhpaChannel)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        ColorPicker picker = new(owner)
        {
            SelectedColor = initial,
            AllowAlpha = allowAlhpaChannel
        };
        Boolean? result = picker.ShowDialog();
        return result.HasValue &&
               result.Value
                    ? picker.SelectedColor
                    : null;
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
    public static Color? Show([DisallowNull] Window owner,
                              in Color initial,
                              in Boolean allowAlhpaChannel,
                              [DisallowNull] Style withStyle)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }
        if (withStyle is null)
        {
            throw new ArgumentNullException(nameof(withStyle));
        }

        ColorPicker picker = new(owner)
        {
            SelectedColor = initial,
            AllowAlpha = allowAlhpaChannel,
            Style = withStyle
        };
        Boolean? result = picker.ShowDialog();
        return result.HasValue &&
               result.Value
                    ? picker.SelectedColor
                    : null;
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
    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(default(Color)));

    /// <summary>
    /// Gets or sets the <see cref="Color"/> that this picker has currently selected.
    /// </summary>
    public Color SelectedColor
    {
        get => (Color)this.GetValue(SelectedColorProperty);
        set => this.SetValue(SelectedColorProperty, value);
    }

    /// <summary>
    /// Gets or sets if the <see cref="Color.A"/> alpha values should be editable.
    /// </summary>
    public Boolean AllowAlpha
    {
        get => m_AllowAlpha &&
               m_ASlider is not null &&
            m_ASlider.Visibility == Visibility.Visible;
        set
        {
            m_AllowAlpha = value;
            if (m_ASlider is null)
            {
                return;
            }
            m_ASlider.Visibility = value
                                          ? Visibility.Visible
                                          : Visibility.Collapsed;
        }
    }
}

// Non-Public
partial class ColorPicker
{
#nullable disable

    private ColorPicker(Window owner)
    {
        if (owner is null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        this.Owner = owner;
        this.InitializeComponent();
    }

#nullable enable

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key is Key.Enter &&
            e.OriginalSource is TextBox box &&
            box == m_HexadecimalText)
        {
            this.SetHexadecimalString(box.Text);
        }
    }

    private void InitializeComponent()
    {
        if (m_ContentLoaded)
        {
            return;
        }

        m_ContentLoaded = true;
        this.Width = 256;
        this.Height = 284;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.ResizeMode = ResizeMode.NoResize;
        this.WindowStyle = WindowStyle.None;
        this.ShowInTaskbar = false;
        ParserContext context = new();
        context.XmlnsDictionary.Add("",
                                    "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
        context.XmlnsDictionary.Add("x",
                                    "http://schemas.microsoft.com/winfx/2006/xaml");
        context.XmlnsDictionary.Add("w",
                                    "clr-namespace:Narumikazuchi.Windows;assembly=Narumikazuchi.Windows");
        const String xaml =
            "<ControlTemplate TargetType=\"{x:Type w:ColorPicker}\">" +
                "<Border Padding=\"3\" BorderThickness=\"1\" BorderBrush=\"{TemplateBinding BorderBrush}\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                "<Border.Resources>" +
                    "<w:ColorToSolidColorBrushConverter x:Key=\"Converter\"/>" +
                    "<Style TargetType=\"{x:Type Slider}\" x:Key=\"ColorSliderStyle\">" +
                        "<Setter Property=\"BorderBrush\" Value=\"DarkGray\"/>" +
                        "<Setter Property=\"BorderThickness\" Value=\"1\"/>" +
                        "<Setter Property=\"SmallChange\" Value=\"1\"/>" +
                        "<Setter Property=\"LargeChange\" Value=\"10\"/>" +
                        "<Setter Property=\"Orientation\" Value=\"Horizontal\"/>" +
                        "<Setter Property=\"Minimum\" Value=\"0\"/>" +
                        "<Setter Property=\"Maximum\" Value=\"255\"/>" +
                        "<Setter Property=\"TickFrequency\" Value=\"1\"/>" +
                        "<Setter Property=\"IsSnapToTickEnabled\" Value=\"True\"/>" +
                        "<Setter Property=\"IsDirectionReversed\" Value=\"False\"/>" +
                        "<Setter Property=\"IsMoveToPointEnabled\" Value=\"True\"/>" +
                        "<Setter Property=\"Value\" Value=\"0\"/>" +
                        "<Setter Property=\"VerticalAlignment\" Value=\"Center\"/>" +
                        "<Setter Property=\"Template\">" +
                            "<Setter.Value>" +
                                "<ControlTemplate TargetType=\"{x:Type Slider}\">" +
                                    "<Grid>" +
                                        "<Border BorderBrush=\"{TemplateBinding BorderBrush}\" BorderThickness=\"{TemplateBinding BorderThickness}\" Margin=\"0 8 0 0\">" +
                                            "<Border x:Name=\"PART_TrackBackground\" Height=\"15\">" +
                                                "<Rectangle Fill=\"{TemplateBinding Background}\" Stretch=\"Fill\" VerticalAlignment=\"Stretch\"/>" +
                                            "</Border>" +
                                        "</Border>" +
                                        "<Track x:Name=\"PART_Track\">" +
                                            "<Track.Resources>" +
                                                "<Style x:Key=\"SliderRepeatButtonStyle\" TargetType=\"{x:Type RepeatButton}\">" +
                                                    "<Setter Property=\"OverridesDefaultStyle\" Value=\"true\" />" +
                                                    "<Setter Property=\"IsTabStop\" Value=\"false\" />" +
                                                    "<Setter Property=\"Focusable\" Value=\"false\" />" +
                                                    "<Setter Property=\"Background\" Value=\"Transparent\" />" +
                                                    "<Setter Property=\"Template\">" +
                                                        "<Setter.Value>" +
                                                            "<ControlTemplate TargetType=\"{x:Type RepeatButton}\">" +
                                                                "<Border Background=\"Transparent\" />" +
                                                            "</ControlTemplate>" +
                                                        "</Setter.Value>" +
                                                    "</Setter>" +
                                                "</Style>" +
                                            "</Track.Resources>" +
                                            "<Track.DecreaseRepeatButton>" +
                                                "<RepeatButton Command=\"Slider.DecreaseLarge\" Style=\"{StaticResource SliderRepeatButtonStyle}\"/>" +
                                            "</Track.DecreaseRepeatButton>" +
                                            "<Track.IncreaseRepeatButton>" +
                                                "<RepeatButton Command=\"Slider.IncreaseLarge\" Style=\"{StaticResource SliderRepeatButtonStyle}\"/>" +
                                            "</Track.IncreaseRepeatButton>" +
                                            "<Track.Thumb>" +
                                                "<Thumb>" +
                                                    "<Thumb.Style>" +
                                                        "<Style TargetType=\"{x:Type Thumb}\">" +
                                                            "<Setter Property=\"Focusable\" Value=\"false\" />" +
                                                            "<Setter Property=\"OverridesDefaultStyle\" Value=\"true\" /> " +
                                                            "<Setter Property=\"Height\" Value=\"15\" /> " +
                                                            "<Setter Property=\"Width\" Value=\"12\" /> " +
                                                            "<Setter Property=\"Foreground\" Value=\"Gray\" /> " +
                                                            "<Setter Property=\"Template\"> " +
                                                                "<Setter.Value>" +
                                                                    "<ControlTemplate TargetType=\"{x:Type Thumb}\">" +
                                                                        "<Canvas SnapsToDevicePixels=\"true\" Background=\"Transparent\">" +
                                                                            "<Path x:Name=\"UpArrow\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"{TemplateBinding Foreground}\" Data=\"F1 M 276.761, 316L 262.619, 307.835L 262.619, 324.165L 276.761, 316 Z\" RenderTransformOrigin=\"0.5, 0.5\" Width=\"6\" Height=\"8\"> " +
                                                                                "<Path.RenderTransform> " +
                                                                                    "<TransformGroup> " +
                                                                                        "<RotateTransform Angle=\"90\" />" +
                                                                                        "<TranslateTransform X=\"6\" /> " +
                                                                                    "</TransformGroup> " +
                                                                                "</Path.RenderTransform> " +
                                                                            "</Path> " +
                                                                        "</Canvas>" +
                                                                    "</ControlTemplate>" +
                                                                "</Setter.Value>" +
                                                            "</Setter>" +
                                                        "</Style>" +
                                                    "</Thumb.Style>" +
                                                "</Thumb>" +
                                            "</Track.Thumb>" +
                                        "</Track>" +
                                    "</Grid>" +
                                "</ControlTemplate>" +
                            "</Setter.Value>" +
                        "</Setter>" +
                    "</Style>" +
                "</Border.Resources>" +
                    "<Grid>" +
                        "<Grid.RowDefinitions>" +
                            "<RowDefinition Height=\"*\"/>" +
                            "<RowDefinition Height=\"40\"/>" +
                        "</Grid.RowDefinitions>" +
                        "<Grid Margin=\"2\">" +
                            "<Grid.RowDefinitions>" +
                                "<RowDefinition Height=\"Auto\"/>" +
                                "<RowDefinition Height=\"Auto\"/>" +
                            "</Grid.RowDefinitions>" +
                            "<Grid Grid.Row=\"0\">" +
                                "<Grid.ColumnDefinitions>" +
                                    "<ColumnDefinition Width=\"200\"/>" +
                                    "<ColumnDefinition Width=\"Auto\"/>" +
                                "</Grid.ColumnDefinitions>" +
                                "<Grid.RowDefinitions>" +
                                    "<RowDefinition Height=\"Auto\"/>" +
                                    "<RowDefinition Height=\"Auto\"/>" +
                                "</Grid.RowDefinitions>" +
                                "<Border Grid.Row=\"0\" BorderThickness=\"1\" BorderBrush=\"DarkGray\" ClipToBounds=\"True\">" +
                                    "<Canvas x:Name=\"" + nameof(m_ColorShadingCanvas) + "\" Width=\"200\" Height=\"100\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">" +
                                        "<Rectangle Width=\"200\" Height=\"100\" Fill=\"{Binding SelectedColor, ElementName=" + nameof(m_SpectrumSlider) + ", Converter={StaticResource Converter}}\" />" +
                                        "<Rectangle Width=\"200\" Height=\"100\">" +
                                            "<Rectangle.Fill>" +
                                                "<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"1,0\">" +
                                                    "<GradientStop Offset=\"0\" Color=\"#FFFFFFFF\"/>" +
                                                    "<GradientStop Offset=\"1\" Color=\"#00FFFFFF\"/>" +
                                                "</LinearGradientBrush>" +
                                            "</Rectangle.Fill>" +
                                        "</Rectangle>" +
                                        "<Rectangle Width=\"200\" Height=\"100\">" +
                                            "<Rectangle.Fill>" +
                                                "<LinearGradientBrush StartPoint=\"0,1\" EndPoint=\"0,0\">" +
                                                    "<GradientStop Offset=\"0\" Color=\"#FF000000\"/>" +
                                                    "<GradientStop Offset=\"1\" Color=\"#00000000\"/>" +
                                                "</LinearGradientBrush>" +
                                            "</Rectangle.Fill>" +
                                        "</Rectangle>" +
                                        "<Canvas x:Name=\"" + nameof(m_ColorShadeSelector) + "\" Width=\"10\" Height=\"10\" IsHitTestVisible=\"False\">" +
                                            "<Ellipse Width=\"10\" Height=\"10\" Stroke=\"#FFFFFFFF\" StrokeThickness=\"3\" IsHitTestVisible=\"False\"/>" +
                                            "<Ellipse Width=\"10\" Height=\"10\" Stroke=\"#FF000000\" StrokeThickness=\"1\" IsHitTestVisible=\"False\"/>" +
                                        "</Canvas>" +
                                    "</Canvas>" +
                                "</Border>" +
                                "<Border Grid.Column=\"0\" Grid.Row=\"1\" Margin=\"0 5 0 0\">" +
                                    "<Grid>" +
                                        "<Grid.ColumnDefinitions>" +
                                            "<ColumnDefinition Width=\"*\"/>" +
                                            "<ColumnDefinition Width=\"*\"/>" +
                                        "</Grid.ColumnDefinitions>" +
                                        "<Border Grid.Column=\"0\" Height=\"22\" Margin=\"2 0\" BorderThickness=\"1\" BorderBrush=\"#FFC9CACA\">" +
                                            "<Rectangle Fill=\"{TemplateBinding SelectedColor, Converter={StaticResource Converter}}\"/>" +
                                        "</Border>" +
                                        "<TextBox x:Name=\"" + nameof(m_HexadecimalText) + "\" Grid.Column=\"1\" Margin=\"2 0\" Foreground=\"{TemplateBinding Foreground}\" Background=\"Transparent\" BorderBrush=\"{TemplateBinding Foreground}\" VerticalAlignment=\"Center\" />" +
                                    "</Grid>" +
                                "</Border>" +
                                "<Border Grid.Column=\"1\" Grid.Row=\"0\" Grid.RowSpan=\"2\" Margin=\"4 -8 0 0\" ClipToBounds=\"False\">" +
                                    "<w:ColorSpectrumSlider x:Name=\"" + nameof(m_SpectrumSlider) + "\" VerticalAlignment=\"Stretch\"/>" +
                                "</Border>" +
                            "</Grid>" +
                            "<Border Grid.Column=\"0\" Grid.Row=\"1\" BorderThickness=\"1\" Margin=\"0 10 0 0\" MinWidth=\"180\" ClipToBounds=\"True\">" +
                                "<Grid>" +
                                    "<Grid.RowDefinitions>" +
                                        "<RowDefinition Height=\"19\"/>" +
                                        "<RowDefinition Height=\"19\"/>" +
                                        "<RowDefinition Height=\"19\"/>" +
                                        "<RowDefinition Height=\"19\"/>" +
                                    "</Grid.RowDefinitions>" +
                                    "<Slider x:Name=\"" + nameof(m_RSlider) + "\" Grid.Row=\"0\" Background=\"#FFFF0000\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                    "<Slider x:Name=\"" + nameof(m_GSlider) + "\" Grid.Row=\"1\" Background=\"#FF00FF00\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                    "<Slider x:Name=\"" + nameof(m_BSlider) + "\" Grid.Row=\"2\" Background=\"#FF0000FF\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                    "<Slider x:Name=\"" + nameof(m_ASlider) + "\" Grid.Row=\"3\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\">" +
                                        "<Slider.Background>" +
                                            "<DrawingBrush Viewport=\"0 0 0.05 0.2\" TileMode=\"Tile\">" +
                                                "<DrawingBrush.Drawing>" +
                                                    "<DrawingGroup>" +
                                                        "<GeometryDrawing Brush=\"LightGray\">" +
                                                            "<GeometryDrawing.Geometry>" +
                                                                "<GeometryGroup>" +
                                                                    "<RectangleGeometry Rect=\"0 0 10 15\"/>" +
                                                                "</GeometryGroup>" +
                                                            "</GeometryDrawing.Geometry>" +
                                                        "</GeometryDrawing>" +
                                                        "<GeometryDrawing Brush=\"DarkGray\">" +
                                                            "<GeometryDrawing.Geometry>" +
                                                                "<GeometryGroup>" +
                                                                    "<RectangleGeometry Rect=\"0 0 5 5\"/>" +
                                                                    "<RectangleGeometry Rect=\"5 5 5 5\"/>" +
                                                                    "<RectangleGeometry Rect=\"0 10 5 5\"/>" +
                                                                "</GeometryGroup>" +
                                                            "</GeometryDrawing.Geometry>" +
                                                        "</GeometryDrawing>" +
                                                    "</DrawingGroup>" +
                                                "</DrawingBrush.Drawing>" +
                                            "</DrawingBrush>" +
                                        "</Slider.Background>" +
                                    "</Slider>" +
                                "</Grid>" +
                            "</Border>" +
                        "</Grid>" +
                        "<StackPanel Grid.Row=\"1\" HorizontalAlignment=\"Right\" Orientation=\"Horizontal\">" +
                            "<Button x:Name=\"_btnOk\" Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsDefault=\"True\" Content=\"OK\"/>" +
                            "<Button Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsCancel=\"True\" Content=\"Cancel\"/>" +
                        "</StackPanel>" +
                    "</Grid>" +
                "</Border>" +
            "</ControlTemplate>";
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(xaml));
        ControlTemplate template = (ControlTemplate)XamlReader.Load(stream,
                                                                    context);
        this.Template = template;
    }

    private void UpdateColorShadeSelectorPosition(Point p,
                                                  in Boolean calculateColor)
    {
        if (m_ColorShadingCanvas is null ||
            m_ColorShadeSelector is null)
        {
            return;
        }

        p.X = p.X.Clamp(0,
                        m_ColorShadingCanvas.ActualWidth);
        p.Y = p.Y.Clamp(0,
                        m_ColorShadingCanvas.ActualHeight);

        m_ColorShadeSelectorTransform.X = p.X - m_ColorShadeSelector.Width / 2;
        m_ColorShadeSelectorTransform.Y = p.Y - m_ColorShadeSelector.Height / 2;

        p.X /= m_ColorShadingCanvas.ActualWidth;
        p.Y /= m_ColorShadingCanvas.ActualHeight;

        m_CurrentColorPosition = p;

        if (calculateColor)
        {
            this.CalculateColor(p);
        }
    }

    private void UpdateColorShadeSelectorPosition(in Color color)
    {
        if (m_SpectrumSlider is null ||
            m_ColorShadingCanvas is null)
        {
            return;
        }

        m_CurrentColorPosition = null;

        HsvColor hsv = HsvColor.FromRgb(color.R,
                                        color.G,
                                        color.B);

        if (m_UpdateSpectrumSliderValue)
        {
            m_SpectrumSlider.Value = 360 - hsv.H;
        }

        Point p = new(hsv.S,
                      1 - hsv.V);
        m_CurrentColorPosition = p;

        m_ColorShadeSelectorTransform.X = p.X * m_ColorShadingCanvas.Width - 5;
        m_ColorShadeSelectorTransform.Y = p.Y * m_ColorShadingCanvas.Height - 5;
    }

    private void CalculateColor(in Point p)
    {
        if (m_SpectrumSlider is null)
        {
            return;
        }

        HsvColor hsv = HsvColor.FromHsv(360 - m_SpectrumSlider.Value,
                                        p.X,
                                        1 - p.Y);
        Color color = (Color)hsv;

        this.SelectedColor = color;
        this.SetHexadecimalStringTextBox(color);
        m_RSlider.Value = color.R;
        m_GSlider.Value = color.G;
        m_BSlider.Value = color.B;
        if (this.AllowAlpha)
        {
            m_ASlider.Value = color.A;
        }
    }

    private void SetHexadecimalString(String newValue)
    {
        if (String.IsNullOrWhiteSpace(newValue))
        {
            this.SetHexadecimalStringTextBox(this.SelectedColor);
            return;
        }

        try
        {
            if (Int32.TryParse(newValue,
                               NumberStyles.HexNumber,
                               null,
                               out Int32 _))
            {
                newValue = "#" + newValue;
            }
            Color color = (Color)ColorConverter.ConvertFromString(newValue);
            this.SetHexadecimalStringTextBox(color);
        }
        catch
        {
            this.SetHexadecimalStringTextBox(this.SelectedColor);
        }
    }

    private void SetHexadecimalStringTextBox(in Color color)
    {
        if (m_HexadecimalText is not null)
        {
            String? value = this.AllowAlpha
                                    ? color.ToString()
                                    : color.ToString()
                                           .Remove(1, 2);
            m_HexadecimalText.Text = value;
        }
    }

    private void ColorShadingCanvas_MouseLeftButtonDown(Object? sender,
                                                        MouseButtonEventArgs e)
    {
        if (m_ColorShadingCanvas is null)
        {
            return;
        }

        Point p = e.GetPosition(m_ColorShadingCanvas);
        this.UpdateColorShadeSelectorPosition(p,
                                              true);
        e.Handled = true;
    }

    private void ColorShadingCanvas_MouseLeftButtonUp(Object? sender,
                                                      MouseButtonEventArgs e)
    {
        if (m_ColorShadingCanvas is null)
        {
            return;
        }
        m_ColorShadingCanvas.ReleaseMouseCapture();
    }

    private void ColorShadingCanvas_MouseMove(Object? sender,
                                              MouseEventArgs e)
    {
        if (m_ColorShadingCanvas is null ||
            e.LeftButton is not MouseButtonState.Pressed)
        {
            return;
        }
        Point p = e.GetPosition(m_ColorShadingCanvas);
        this.UpdateColorShadeSelectorPosition(p,
                                              true);
        Mouse.Synchronize();
    }

    private void ColorShadingCanvas_SizeChanged(Object? sender,
                                                SizeChangedEventArgs e)
    {
        if (m_CurrentColorPosition is null)
        {
            return;
        }
        Point newPoint = new()
        {
            X = ((Point)m_CurrentColorPosition).X * e.NewSize.Width,
            Y = ((Point)m_CurrentColorPosition).Y * e.NewSize.Height
        };

        this.UpdateColorShadeSelectorPosition(newPoint,
                                              false);
    }

    private void SpectrumSlider_ValueChanged(Object? sender,
                                             RoutedPropertyChangedEventArgs<Double> e)
    {
        if (m_CurrentColorPosition is null)
        {
            return;
        }
        this.CalculateColor((Point)m_CurrentColorPosition);
    }

    private void HexadecimalText_LostFocus(Object? sender,
                                           RoutedEventArgs e)
    {
        if (sender is null ||
            sender is not TextBox textbox)
        {
            return;
        }
        // Update SelectedColor to input string
        this.SetHexadecimalString(textbox.Text);
    }

    private void RgbaSlider_ValueChanged(Object? sender,
                                         RoutedPropertyChangedEventArgs<Double> e)
    {
        if (sender is not Slider slider)
        {
            return;
        }

        if (slider == m_RSlider)
        {
            Color color = Color.FromArgb(this.SelectedColor.A,
                                         (Byte)m_RSlider.Value.Clamp(0.0d,
                                                                     255.0d),
                                         this.SelectedColor.G,
                                         this.SelectedColor.B);
            this.UpdateColorShadeSelectorPosition(color);
            this.SetHexadecimalStringTextBox(color);
            this.SelectedColor = color;
        }
        else if (slider == m_GSlider)
        {
            Color color = Color.FromArgb(this.SelectedColor.A,
                                         this.SelectedColor.R,
                                         (Byte)m_GSlider.Value.Clamp(0.0d,
                                                                     255.0d),
                                         this.SelectedColor.B);
            this.UpdateColorShadeSelectorPosition(color);
            this.SetHexadecimalStringTextBox(color);
            this.SelectedColor = color;
        }
        else if (slider == m_BSlider)
        {
            Color color = Color.FromArgb(this.SelectedColor.A,
                                         this.SelectedColor.R,
                                         this.SelectedColor.G,
                                         (Byte)m_BSlider.Value.Clamp(0.0d,
                                                                     255.0d));
            this.UpdateColorShadeSelectorPosition(color);
            this.SetHexadecimalStringTextBox(color);
            this.SelectedColor = color;
        }
        else if (slider == m_ASlider)
        {
            Color color = Color.FromArgb((Byte)m_ASlider.Value.Clamp(0.0d,
                                                                     255.0d),
                                         this.SelectedColor.R,
                                         this.SelectedColor.G,
                                         this.SelectedColor.B);
            this.UpdateColorShadeSelectorPosition(color);
            this.SetHexadecimalStringTextBox(color);
            this.SelectedColor = color;
        }
    }

    private void Ok_Click(Object sender,
                          RoutedEventArgs e) =>
        this.DialogResult = true;

    private T GetTemplateChild<T>(String childName)
        where T : DependencyObject =>
            this.GetTemplateChild(childName) is T result
                ? result
                : throw new InvalidCastException();

    private readonly TranslateTransform m_ColorShadeSelectorTransform = new();
    private readonly Boolean m_UpdateSpectrumSliderValue = true;
    private Boolean m_ContentLoaded = false;
    private Boolean m_AllowAlpha = true;
    private Canvas m_ColorShadingCanvas;
    private Canvas m_ColorShadeSelector;
    private ColorSpectrumSlider m_SpectrumSlider;
    private TextBox m_HexadecimalText;
    private Slider m_RSlider;
    private Slider m_GSlider;
    private Slider m_BSlider;
    private Slider m_ASlider;
    private Button m_BtnOk;
    private Point? m_CurrentColorPosition;
}