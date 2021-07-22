using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents the spectrum slider next to the color canvas on a <see cref="ColorPicker"/> object.
    /// </summary>
    [TemplatePart(Name = PART_SpectrumDisplay, Type = typeof(Rectangle))]
    public sealed class ColorSpectrumSlider : Slider
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSpectrumSlider"/> class.
        /// </summary>
        public ColorSpectrumSlider() => this.InitializeComponent();

        #endregion

        #region Initialize

        private void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }
            this._contentLoaded = true;

            #region Slider
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
            #endregion
            #region Template
            ParserContext context = new();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("w", "clr-namespace:Narumikazuchi.Windows;assembly=Narumikazuchi.Windows");
            const String xaml =
                "<ControlTemplate TargetType=\"{x:Type w:ColorSpectrumSlider}\">" +
                    "<Grid>" +
                        "<Border BorderBrush=\"{TemplateBinding BorderBrush}\" BorderThickness=\"{TemplateBinding BorderThickness}\" Margin=\"0 8 0 0\">" +
                            "<Border x:Name=\"PART_TrackBackground\" Width=\"15\">" +
                                "<Rectangle x:Name=\"" + PART_SpectrumDisplay + "\" Stretch=\"Fill\" VerticalAlignment=\"Stretch\"/>" +
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
            ControlTemplate template = (ControlTemplate)XamlReader.Load(stream, context);
            this.Template = template;
            #endregion
        }

        private void CreateSpectrum()
        {
            this._pickerBrush = new()
            {
                StartPoint = new(0.5, 0),
                EndPoint = new(0.5, 1),
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation
            };

            const Double increment = 0.142857;

            Int32 i;
            for (i = 0; i < HsvSpectrum.Count; i++)
            {
                this._pickerBrush.GradientStops.Add(new(HsvSpectrum[i], i * increment));
            }
            this._pickerBrush.GradientStops[i - 1].Offset = 1.0;

            if (this._spectrumDisplay is not null)
            {
                this._spectrumDisplay.Fill = this._pickerBrush;
            }
        }

        #endregion

        #region Slider

        private T GetTemplateChild<T>(String childName) where T : DependencyObject => this.GetTemplateChild(childName) is T result ? result : throw new InvalidCastException();

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._spectrumDisplay = this.GetTemplateChild<Rectangle>(PART_SpectrumDisplay);
            this.CreateSpectrum();
            this.OnValueChanged(Double.NaN, this.Value);
        }

        /// <inheritdoc/>
        protected override void OnValueChanged(Double oldValue, Double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            HsvColor hsv = HsvColor.FromHsv(360 - newValue, 1, 1);
            Color color = (Color)hsv;
            this.SelectedColor = color;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Color),
                typeof(ColorSpectrumSlider),
                new FrameworkPropertyMetadata(Colors.White));
        /// <summary>
        /// Gets or sets the currently selected <see cref="Color"/>.
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)this.GetValue(SelectedColorProperty);
            set => this.SetValue(SelectedColorProperty, value);
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

        #endregion

        #region Fields

        private Boolean _contentLoaded = false;
        private Rectangle? _spectrumDisplay;
        private LinearGradientBrush? _pickerBrush;

        #endregion

        #region Constants

        private const String PART_SpectrumDisplay = "PART_SpectrumDisplay";

        #endregion
    }
}
