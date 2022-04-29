using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Orientation = System.Windows.Controls.Orientation;
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
        get => (Color)this.GetValue(SelectedColorProperty);
        set => this.SetValue(dp: SelectedColorProperty,
                             value: value);
    }
}

// Non-Public
partial class ColorSpectrumSlider : Slider
{
    /// <inheritdoc/>
    protected override void OnValueChanged(Double oldValue,
                                           Double newValue)
    {
        base.OnValueChanged(oldValue: oldValue,
                            newValue: newValue);

        HsvColor hsv = HsvColor.FromHsv(hue: 360 - newValue,
                                        saturation: 1,
                                        value: 1);
        Color color = (Color)hsv;
        this.SelectedColor = color;
    }

    private void InitializeComponent()
    {
        if (m_ContentLoaded)
        {
            return;
        }
        m_ContentLoaded = true;

        this.BorderBrush = Brushes.DarkGray;
        this.BorderThickness = new(1);
        this.SmallChange = 10;
        this.Orientation = Orientation.Vertical;
        this.Background = Brushes.Transparent;
        this.Minimum = 0;
        this.Maximum = 360;
        this.TickFrequency = 0.001d;
        this.IsSnapToTickEnabled = true;
        this.IsDirectionReversed = false;
        this.IsMoveToPointEnabled = true;
        this.Value = 0;
        ParserContext context = new();
        context.XmlnsDictionary.Add(prefix: "",
                                    xmlNamespace: "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
        context.XmlnsDictionary.Add(prefix: "x",
                                    xmlNamespace: "http://schemas.microsoft.com/winfx/2006/xaml");
        context.XmlnsDictionary.Add(prefix: "w",
                                    xmlNamespace: "clr-namespace:Narumikazuchi.Windows;assembly=Narumikazuchi.Windows");
        const String xaml =
            "<ControlTemplate TargetType=\"{x:Type w:ColorSpectrumSlider}\">" +
                "<Grid>" +
                    "<Border BorderBrush=\"{TemplateBinding BorderBrush}\" BorderThickness=\"{TemplateBinding BorderThickness}\" Margin=\"0 8 0 0\">" +
                        "<Border x:Name=\"PART_TrackBackground\" Width=\"15\">" +
                            "<Rectangle x:Name=\"" + PART_SPECTRUMDISPLAY + "\" Stretch=\"Fill\" VerticalAlignment=\"Stretch\"/>" +
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
                                            "<Border Background=\"{TemplateBinding Background}\" />" +
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
                                        "<Setter Property=\"Height\" Value=\"12\" /> " +
                                        "<Setter Property=\"Width\" Value=\"15\" /> " +
                                        "<Setter Property=\"Foreground\" Value=\"Gray\" /> " +
                                        "<Setter Property=\"Template\"> " +
                                            "<Setter.Value>" +
                                                "<ControlTemplate TargetType=\"{x:Type Thumb}\">" +
                                                    "<Canvas SnapsToDevicePixels=\"true\" Background=\"Transparent\">" +
                                                        "<Path x:Name=\"LeftArrow\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FF000000\" Data=\"F1 M 276.761, 316L 262.619, 307.835L 262.619, 324.165L 276.761, 316 Z\" RenderTransformOrigin=\"0.5, 0.5\" Width=\"6\" Height=\"8\"> " +
                                                            "<Path.RenderTransform> " +
                                                                "<TransformGroup> " +
                                                                    "<TranslateTransform Y=\"6\" /> " +
                                                                "</TransformGroup> " +
                                                            "</Path.RenderTransform> " +
                                                        "</Path> " +
                                                        "<Path x:Name=\"RightArrow\" Stretch=\"Fill\" StrokeLineJoin=\"Round\" Stroke=\"#FF000000\" Fill=\"#FF000000\" Data=\"F1 M 276.761, 316L 262.619, 307.835L 262.619, 324.165L 276.761, 316 Z\" RenderTransformOrigin=\"0.5, 0.5\" Width=\"6\" Height=\"8\"> " +
                                                            "<Path.RenderTransform> " +
                                                                "<TransformGroup> " +
                                                                    "<RotateTransform Angle=\"-180\" />" +
                                                                    "<TranslateTransform Y=\"6\" X=\"9\" />" +
                                                                "</TransformGroup>" +
                                                            "</Path.RenderTransform>" +
                                                        "</Path>" +
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
            "</ControlTemplate>";
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(xaml));
        ControlTemplate template = (ControlTemplate)XamlReader.Load(stream: stream,
                                                                    parserContext: context);
        this.Template = template;
    }

    private void CreateSpectrum()
    {
        m_PickerBrush = new()
        {
            StartPoint = new(x: 0.5, y: 0),
            EndPoint = new(x: 0.5, y: 1),
            ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation
        };

        const Double increment = 0.142857;

        Int32 i;
        for (i = 0; i < HsvSpectrum.Count; i++)
        {
            m_PickerBrush.GradientStops.Add(new(color: HsvSpectrum[i],
                                                offset: i * increment));
        }
        m_PickerBrush.GradientStops[i - 1].Offset = 1.0;

        if (m_SpectrumDisplay is not null)
        {
            m_SpectrumDisplay.Fill = m_PickerBrush;
        }
    }

    private T GetTemplateChild<T>(String childName)
        where T : DependencyObject
    {
        if (this.GetTemplateChild(childName) is T result)
        {
            return result;
        }
        throw new InvalidCastException();
    }

    private static IReadOnlyList<Color> HsvSpectrum { get; } = new List<Color>()
        {
            (Color)HsvColor.FromHsv(0, 1, 1),
            (Color)HsvColor.FromHsv(60, 1, 1),
            (Color)HsvColor.FromHsv(120, 1, 1),
            (Color)HsvColor.FromHsv(180, 1, 1),
            (Color)HsvColor.FromHsv(240, 1, 1),
            (Color)HsvColor.FromHsv(300, 1, 1),
            (Color)HsvColor.FromHsv(360, 1, 1),
            (Color)HsvColor.FromHsv(0, 1, 1)
        };

    private Boolean m_ContentLoaded = false;
    private Rectangle? m_SpectrumDisplay;
    private LinearGradientBrush? m_PickerBrush;

    private const String PART_SPECTRUMDISPLAY = "PART_SpectrumDisplay";
}