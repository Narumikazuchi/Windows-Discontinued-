using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
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

            if (this._colorShadingCanvas is not null)
            {
                this._colorShadingCanvas.MouseLeftButtonDown -= this.ColorShadingCanvas_MouseLeftButtonDown;
                this._colorShadingCanvas.MouseLeftButtonUp -= this.ColorShadingCanvas_MouseLeftButtonUp;
                this._colorShadingCanvas.MouseMove -= this.ColorShadingCanvas_MouseMove;
                this._colorShadingCanvas.SizeChanged -= this.ColorShadingCanvas_SizeChanged;
            }

            this._colorShadingCanvas = this.GetTemplateChild<Canvas>(nameof(this._colorShadingCanvas));

            if (this._colorShadingCanvas is not null)
            {
                this._colorShadingCanvas.MouseLeftButtonDown += this.ColorShadingCanvas_MouseLeftButtonDown;
                this._colorShadingCanvas.MouseLeftButtonUp += this.ColorShadingCanvas_MouseLeftButtonUp;
                this._colorShadingCanvas.MouseMove += this.ColorShadingCanvas_MouseMove;
                this._colorShadingCanvas.SizeChanged += this.ColorShadingCanvas_SizeChanged;
            }

            this._colorShadeSelector = this.GetTemplateChild<Canvas>(nameof(this._colorShadeSelector));

            if (this._colorShadeSelector is not null)
            {
                this._colorShadeSelector.RenderTransform = this._colorShadeSelectorTransform;
            }

            if (this._spectrumSlider is not null)
            {
                this._spectrumSlider.ValueChanged -= this.SpectrumSlider_ValueChanged;
            }

            this._spectrumSlider = this.GetTemplateChild<ColorSpectrumSlider>(nameof(this._spectrumSlider));

            if (this._spectrumSlider is not null)
            {
                this._spectrumSlider.ValueChanged += this.SpectrumSlider_ValueChanged;
            }

            if (this._hexadecimalText is not null)
            {
                this._hexadecimalText.LostFocus -= this.HexadecimalText_LostFocus;
            }

            this._hexadecimalText = this.GetTemplateChild<TextBox>(nameof(this._hexadecimalText));

            if (this._hexadecimalText is not null)
            {
                this._hexadecimalText.LostFocus += this.HexadecimalText_LostFocus;
            }

            if (this._rSlider is not null)
            {
                this._rSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
            }

            this._rSlider = this.GetTemplateChild<Slider>(nameof(this._rSlider));

            if (this._rSlider is not null)
            {
                this._rSlider.ValueChanged += this.RgbaSlider_ValueChanged;
            }

            if (this._gSlider is not null)
            {
                this._gSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
            }

            this._gSlider = this.GetTemplateChild<Slider>(nameof(this._gSlider));

            if (this._gSlider is not null)
            {
                this._gSlider.ValueChanged += this.RgbaSlider_ValueChanged;
            }

            if (this._bSlider is not null)
            {
                this._bSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
            }

            this._bSlider = this.GetTemplateChild<Slider>(nameof(this._bSlider));

            if (this._bSlider is not null)
            {
                this._bSlider.ValueChanged += this.RgbaSlider_ValueChanged;
            }

            if (this._aSlider is not null)
            {
                this._aSlider.ValueChanged -= this.RgbaSlider_ValueChanged;
            }

            this._aSlider = this.GetTemplateChild<Slider>(nameof(this._aSlider));

            if (this._aSlider is not null)
            {
                this._aSlider.ValueChanged += this.RgbaSlider_ValueChanged;
                this._aSlider.Visibility = this._allowAlpha ? Visibility.Visible : Visibility.Collapsed;
            }

            if (this._btnOk is not null)
            {
                this._btnOk.Click -= this.Ok_Click;
            }
            this._btnOk = this.GetTemplateChild<Button>(nameof(this._btnOk));
            this._btnOk.Click += this.Ok_Click;

            this.UpdateColorShadeSelectorPosition(this.SelectedColor);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> property.
        /// </summary>
        [Pure]
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
            get => this._allowAlpha && 
                   this._aSlider is not null && 
                this._aSlider.Visibility == Visibility.Visible;
            set
            {
                this._allowAlpha = value;
                if (this._aSlider is null)
                {
                    return;
                }
                this._aSlider.Visibility = value 
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
                box == this._hexadecimalText)
            {
                this.SetHexadecimalString(box.Text);
            }
        }

        private void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }

            this._contentLoaded = true;
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
                                        "<Canvas x:Name=\"" + nameof(this._colorShadingCanvas) + "\" Width=\"200\" Height=\"100\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">" +
                                            "<Rectangle Width=\"200\" Height=\"100\" Fill=\"{Binding SelectedColor, ElementName=" + nameof(this._spectrumSlider) + ", Converter={StaticResource Converter}}\" />" +
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
                                            "<Canvas x:Name=\"" + nameof(this._colorShadeSelector) + "\" Width=\"10\" Height=\"10\" IsHitTestVisible=\"False\">" +
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
                                            "<TextBox x:Name=\"" + nameof(this._hexadecimalText) + "\" Grid.Column=\"1\" Margin=\"2 0\" Foreground=\"{TemplateBinding Foreground}\" Background=\"Transparent\" BorderBrush=\"{TemplateBinding Foreground}\" VerticalAlignment=\"Center\" />" +
                                        "</Grid>" +
                                    "</Border>" +
                                    "<Border Grid.Column=\"1\" Grid.Row=\"0\" Grid.RowSpan=\"2\" Margin=\"4 -8 0 0\" ClipToBounds=\"False\">" +
                                        "<w:ColorSpectrumSlider x:Name=\"" + nameof(this._spectrumSlider) + "\" VerticalAlignment=\"Stretch\"/>" +
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
                                        "<Slider x:Name=\"" + nameof(this._rSlider) + "\" Grid.Row=\"0\" Background=\"#FFFF0000\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                        "<Slider x:Name=\"" + nameof(this._gSlider) + "\" Grid.Row=\"1\" Background=\"#FF00FF00\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                        "<Slider x:Name=\"" + nameof(this._bSlider) + "\" Grid.Row=\"2\" Background=\"#FF0000FF\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\"/>" +
                                        "<Slider x:Name=\"" + nameof(this._aSlider) + "\" Grid.Row=\"3\" Margin=\"4 2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\" Style=\"{StaticResource ColorSliderStyle}\">" +
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
            if (this._colorShadingCanvas is null ||
                this._colorShadeSelector is null)
            {
                return;
            }

            p.X = p.X.Clamp(0, 
                            this._colorShadingCanvas.ActualWidth);
            p.Y = p.Y.Clamp(0, 
                            this._colorShadingCanvas.ActualHeight);

            this._colorShadeSelectorTransform.X = p.X - (this._colorShadeSelector.Width / 2);
            this._colorShadeSelectorTransform.Y = p.Y - (this._colorShadeSelector.Height / 2);

            p.X /= this._colorShadingCanvas.ActualWidth;
            p.Y /= this._colorShadingCanvas.ActualHeight;

            this._currentColorPosition = p;

            if (calculateColor)
            {
                this.CalculateColor(p);
            }
        }
        private void UpdateColorShadeSelectorPosition(in Color color)
        {
            if (this._spectrumSlider is null ||
                this._colorShadingCanvas is null)
            {
                return;
            }

            this._currentColorPosition = null;

            HsvColor hsv = HsvColor.FromRgb(color.R, 
                                            color.G, 
                                            color.B);

            if (this._updateSpectrumSliderValue)
            {
                this._spectrumSlider.Value = 360 - hsv.H;
            }

            Point p = new(hsv.S, 
                          1 - hsv.V);
            this._currentColorPosition = p;

            this._colorShadeSelectorTransform.X = (p.X * this._colorShadingCanvas.Width) - 5;
            this._colorShadeSelectorTransform.Y = (p.Y * this._colorShadingCanvas.Height) - 5;
        }

        private void CalculateColor(in Point p)
        {
            if (this._spectrumSlider is null)
            {
                return;
            }

            HsvColor hsv = HsvColor.FromHsv(360 - this._spectrumSlider.Value, 
                                            p.X, 
                                            1 - p.Y);
            Color color = (Color)hsv;

            this.SelectedColor = color;
            this.SetHexadecimalStringTextBox(color);
            this._rSlider.Value = color.R;
            this._gSlider.Value = color.G;
            this._bSlider.Value = color.B;
            if (this.AllowAlpha)
            {
                this._aSlider.Value = color.A;
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
            if (this._hexadecimalText is not null)
            {
                String? value = this.AllowAlpha 
                                        ? color.ToString() 
                                        : color.ToString()
                                               .Remove(1, 2);
                this._hexadecimalText.Text = value;
            }
        }

        private void ColorShadingCanvas_MouseLeftButtonDown(Object? sender, 
                                                            MouseButtonEventArgs e)
        {
            if (this._colorShadingCanvas is null)
            {
                return;
            }

            Point p = e.GetPosition(this._colorShadingCanvas);
            this.UpdateColorShadeSelectorPosition(p, 
                                                  true);
            e.Handled = true;
        }

        private void ColorShadingCanvas_MouseLeftButtonUp(Object? sender, 
                                                          MouseButtonEventArgs e)
        {
            if (this._colorShadingCanvas is null)
            {
                return;
            }
            this._colorShadingCanvas.ReleaseMouseCapture();
        }

        private void ColorShadingCanvas_MouseMove(Object? sender, 
                                                  MouseEventArgs e)
        {
            if (this._colorShadingCanvas is null ||
                e.LeftButton is not MouseButtonState.Pressed)
            {
                return;
            }
            Point p = e.GetPosition(this._colorShadingCanvas);
            this.UpdateColorShadeSelectorPosition(p, 
                                                  true);
            Mouse.Synchronize();
        }

        private void ColorShadingCanvas_SizeChanged(Object? sender, 
                                                    SizeChangedEventArgs e)
        {
            if (this._currentColorPosition is null)
            {
                return;
            }
            Point newPoint = new()
            {
                X = ((Point)this._currentColorPosition).X * e.NewSize.Width,
                Y = ((Point)this._currentColorPosition).Y * e.NewSize.Height
            };

            this.UpdateColorShadeSelectorPosition(newPoint, 
                                                  false);
        }

        private void SpectrumSlider_ValueChanged(Object? sender, 
                                                 RoutedPropertyChangedEventArgs<Double> e)
        {
            if (this._currentColorPosition is null)
            {
                return;
            }
            this.CalculateColor((Point)this._currentColorPosition);
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

            if (slider == this._rSlider)
            {
                Color color = Color.FromArgb(this.SelectedColor.A, 
                                             (Byte)this._rSlider.Value.Clamp(0.0d, 
                                                                             255.0d), 
                                             this.SelectedColor.G, 
                                             this.SelectedColor.B);
                this.UpdateColorShadeSelectorPosition(color);
                this.SetHexadecimalStringTextBox(color);
                this.SelectedColor = color;
            }
            else if (slider == this._gSlider)
            {
                Color color = Color.FromArgb(this.SelectedColor.A, 
                                             this.SelectedColor.R, 
                                             (Byte)this._gSlider.Value.Clamp(0.0d, 
                                                                             255.0d), 
                                             this.SelectedColor.B);
                this.UpdateColorShadeSelectorPosition(color);
                this.SetHexadecimalStringTextBox(color);
                this.SelectedColor = color;
            }
            else if (slider == this._bSlider)
            {
                Color color = Color.FromArgb(this.SelectedColor.A, 
                                             this.SelectedColor.R, 
                                             this.SelectedColor.G, 
                                             (Byte)this._bSlider.Value.Clamp(0.0d, 
                                                                             255.0d));
                this.UpdateColorShadeSelectorPosition(color);
                this.SetHexadecimalStringTextBox(color);
                this.SelectedColor = color;
            }
            else if (slider == this._aSlider)
            {
                Color color = Color.FromArgb((Byte)this._aSlider.Value.Clamp(0.0d, 
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

        private Boolean _contentLoaded = false;
        private Boolean _allowAlpha = true;
        private TranslateTransform _colorShadeSelectorTransform = new();
        private Boolean _updateSpectrumSliderValue = true;
        private Canvas _colorShadingCanvas;
        private Canvas _colorShadeSelector;
        private ColorSpectrumSlider _spectrumSlider;
        private TextBox _hexadecimalText;
        private Slider _rSlider;
        private Slider _gSlider;
        private Slider _bSlider;
        private Slider _aSlider;
        private Button _btnOk;
        private Point? _currentColorPosition;
    }
}
