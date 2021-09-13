using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Represents a window which can select a <see cref="Font"/>.
    /// </summary>
    public sealed partial class FontPicker : Window
    {
        /// <summary>
        /// Shows the <see cref="FontPicker"/> window and returns the <see cref="Font"/> the user selected.
        /// </summary>
        /// <param name="owner">The owning window of the <see cref="FontPicker"/>.</param>
        /// <returns>The <see cref="Font"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
        /// <exception cref="ArgumentNullException"/>
        [return: MaybeNull]
        public static Font? Show([DisallowNull] Window owner)
        {
            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            FontPicker picker = new(owner)
            {
                SelectedFont = Font.Default
            };
            Boolean? result = picker.ShowDialog();
            return result.HasValue &&
                   result.Value
                    ? picker.SelectedFont
                    : null;
        }
        /// <summary>
        /// Shows the <see cref="FontPicker"/> window and returns the <see cref="Font"/> the user selected.
        /// </summary>
        /// <param name="owner">The owning window of the <see cref="FontPicker"/>.</param>
        /// <param name="initial">The initally selected <see cref="Font"/>.</param>
        /// <returns>The <see cref="Font"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
        /// <exception cref="ArgumentNullException"/>
        [return: MaybeNull]
        public static Font? Show([DisallowNull] Window owner, 
                                 in Font initial)
        {
            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            FontPicker picker = new(owner)
            {
                SelectedFont = initial
            };
            Boolean? result = picker.ShowDialog();
            return result.HasValue &&
                   result.Value
                    ? picker.SelectedFont
                    : null;
        }
        /// <summary>
        /// Shows the <see cref="FontPicker"/> window and returns the <see cref="Font"/> the user selected.
        /// </summary>
        /// <param name="owner">The owning window of the <see cref="FontPicker"/>.</param>
        /// <param name="withStyle">The style to apply to the <see cref="FontPicker"/>.</param>
        /// <returns>The <see cref="Font"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
        /// <exception cref="ArgumentNullException"/>
        [return: MaybeNull]
        public static Font? Show([DisallowNull] Window owner, 
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

            FontPicker picker = new(owner)
            {
                SelectedFont = Font.Default,
                Style = withStyle
            };
            Boolean? result = picker.ShowDialog();
            return result.HasValue &&
                   result.Value
                    ? picker.SelectedFont
                    : null;
        }
        /// <summary>
        /// Shows the <see cref="FontPicker"/> window and returns the <see cref="Font"/> the user selected.
        /// </summary>
        /// <param name="owner">The owning window of the <see cref="FontPicker"/>.</param>
        /// <param name="initial">The initally selected <see cref="Font"/>.</param>
        /// <param name="withStyle">The style to apply to the <see cref="FontPicker"/>.</param>
        /// <returns>The <see cref="Font"/> the user selected or <see langword="null"/>, if the selection was cancelled.</returns>
        /// <exception cref="ArgumentNullException"/>
        [return: MaybeNull]
        public static Font? Show([DisallowNull] Window owner, 
                                 in Font initial, 
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

            FontPicker picker = new(owner)
            {
                SelectedFont = initial,
                Style = withStyle
            };
            Boolean? result = picker.ShowDialog();
            return result.HasValue &&
                   result.Value
                    ? picker.SelectedFont
                    : null;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this._lstFamily is not null)
            {
                this._lstFamily.SelectionChanged -= this.Family_SelectionChanged;
            }
            this._lstFamily = this.GetTemplateChild<ListBox>(nameof(this._lstFamily));
            this._lstFamily.SelectionChanged += this.Family_SelectionChanged;

            if (this._lstTypefaces is not null)
            {
                this._lstTypefaces.SelectionChanged -= this.Typefaces_SelectionChanged;
            }
            this._lstTypefaces = this.GetTemplateChild<ListBox>(nameof(this._lstTypefaces));
            this._lstTypefaces.SelectionChanged += this.Typefaces_SelectionChanged;

            this._sampleText = this.GetTemplateChild<TextBox>(nameof(this._sampleText));

            if (this._fontSizeSlider is not null)
            {
                this._fontSizeSlider.ValueChanged -= this.SizeSlider_ValueChanged;
            }
            this._fontSizeSlider = this.GetTemplateChild<Slider>(nameof(this._fontSizeSlider));
            this._fontSizeSlider.ValueChanged += this.SizeSlider_ValueChanged;

            if (this._btnOk is not null)
            {
                this._btnOk.Click -= this.Ok_Click;
            }
            this._btnOk = this.GetTemplateChild<Button>(nameof(this._btnOk));
            this._btnOk.Click += this.Ok_Click;

            this._lstFamily.ItemsSource = Fonts.SystemFontFamilies;
            this._lstTypefaces.ItemsSource = this.SelectedFont.Family.GetTypefaces();
            this._lstFamily.SelectedItem = this.SelectedFont.Family;
            this._lstFamily.ScrollIntoView(this._lstFamily.SelectedItem);
            this._lstTypefaces.SelectedItem = this.SelectedFont.Typeface;
            this._fontSizeSlider.Value = this.SelectedFont.Size;
            this._sampleText.DataContext = this.SelectedFont;
        }

        /// <summary>
        /// Identifies the <see cref="SelectedFont"/> property.
        /// </summary>
        [Pure]
        [NotNull]
        public static readonly DependencyProperty SelectedFontProperty =
            DependencyProperty.Register(
                nameof(SelectedFont),
                typeof(Font),
                typeof(FontPicker),
                new PropertyMetadata(Font.Default));
        /// <summary>
        /// Gets or sets the <see cref="Font"/> that this picker has currently selected.
        /// </summary>
        public Font SelectedFont
        {
            get => (Font)this.GetValue(SelectedFontProperty);
            set => this.SetValue(SelectedFontProperty, 
                                 value);
        }
    }

    // Non-Public
    partial class FontPicker
    {
#nullable disable
        private FontPicker(Window owner)
        {
            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            this.Owner = owner;
            this.InitializeComponent();
        }
#nullable enable

        private void InitializeComponent()
        {
            if (this._contentLoaded)
            {
                return;
            }

            this._contentLoaded = true;
            this.Width = 592;
            this.Height = 380;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
            this.ShowInTaskbar = false;
            ParserContext context = new();
            context.XmlnsDictionary.Add("", 
                                        "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", 
                                        "http://schemas.microsoft.com/winfx/2006/xaml");
            const String xaml =
                "<ControlTemplate TargetType=\"{x:Type Window}\">" +
                    "<Border BorderThickness=\"1\" BorderBrush=\"{TemplateBinding BorderBrush}\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                        "<Grid>" +
                            "<Grid.RowDefinitions>" +
                                "<RowDefinition Height=\"*\"/>" +
                                "<RowDefinition Height=\"40\"/>" +
                            "</Grid.RowDefinitions>" +
                            "<Grid>" +
                                "<Grid.RowDefinitions>" +
                                    "<RowDefinition Height=\"25\"/>" +
                                    "<RowDefinition Height=\"*\"/>" +
                                "</Grid.RowDefinitions>" +
                                "<Grid.ColumnDefinitions>" +
                                    "<ColumnDefinition Width=\"180\"/>" +
                                    "<ColumnDefinition Width=\"200\"/>" +
                                    "<ColumnDefinition Width=\"*\"/>" +
                                "</Grid.ColumnDefinitions>" +
                                "<TextBlock Grid.Column=\"0\" Grid.Row=\"0\" Foreground=\"{TemplateBinding Foreground}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Bottom\" Padding=\"5\" FontStyle=\"Italic\" Text=\"Font Family\"/>" +
                                "<ListBox x:Name=\"" + nameof(this._lstFamily) + "\" Grid.Column=\"0\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                                    "<ListBox.ItemTemplate>" +
                                        "<DataTemplate>" +
                                            "<TextBlock Text=\"{Binding Source}\" />" +
                                        "</DataTemplate>" +
                                    "</ListBox.ItemTemplate>" +
                                "</ListBox>" +
                                "<TextBlock Grid.Column=\"1\" Grid.Row=\"0\" Foreground=\"{TemplateBinding Foreground}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Bottom\" Padding=\"5\" FontStyle=\"Italic\" Text=\"Typeface\"/>" +
                                "<ListBox x:Name=\"" + nameof(this._lstTypefaces) + "\" Grid.Column=\"1\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                                    "<ListBox.ItemTemplate>" +
                                        "<DataTemplate>" +
                                            "<WrapPanel>" +
                                                "<TextBlock Text=\"{Binding Style}\" />" +
                                                "<TextBlock Text=\"-\" />" +
                                                "<TextBlock Text=\"{Binding Weight}\" />" +
                                                "<TextBlock Text=\"-\" />" +
                                                "<TextBlock Text=\"{Binding Stretch}\" />" +
                                            "</WrapPanel>" +
                                        "</DataTemplate>" +
                                    "</ListBox.ItemTemplate>" +
                                "</ListBox>" +
                                "<TextBlock Grid.Column=\"2\" Grid.Row=\"0\" Foreground=\"{TemplateBinding Foreground}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Bottom\" Padding=\"5\" FontStyle=\"Italic\" Text=\"Font Size\"/>" +
                                "<Grid Grid.Column=\"2\" Grid.Row=\"1\" Margin=\"5\">" +
                                    "<Grid.RowDefinitions>" +
                                        "<RowDefinition Height=\"*\"/>" +
                                        "<RowDefinition Height=\"Auto\"/>" +
                                    "</Grid.RowDefinitions>" +
                                    "<TextBox x:Name=\"" + nameof(this._sampleText) + "\" Grid.Row=\"0\" Background=\"{TemplateBinding Background}\" Foreground=\"{TemplateBinding Foreground}\" IsReadOnly=\"True\" Text=\"Lorem ipsum dolor sit amet, consectetur adipisicing elit\" TextAlignment=\"Center\" TextWrapping=\"Wrap\" FontSize=\"{Binding Size}\" FontFamily=\"{Binding Family}\" FontStretch=\"{Binding Stretch}\" FontStyle=\"{Binding Style}\" FontWeight=\"{Binding Weight}\"/>" +
                                    "<Slider x:Name=\"" + nameof(this._fontSizeSlider) + "\" Grid.Row=\"1\" Minimum=\"8\" Maximum=\"24\" Value=\"12\" SmallChange=\"0.5\" LargeChange=\"2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\"/>" +
                                "</Grid>" +
                            "</Grid>" +
                            "<StackPanel Grid.Row=\"1\" HorizontalAlignment=\"Right\" Orientation=\"Horizontal\">" +
                                "<Button x:Name=\"" + nameof(this._btnOk) + "\" Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsDefault=\"True\" Content=\"OK\"/>" +
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

        private void Family_SelectionChanged(Object sender, 
                                             SelectionChangedEventArgs e)
        {
            FontFamily family = (FontFamily)this._lstFamily.SelectedItem;
            if (family == this.SelectedFont.Family ||
                family is null)
            {
                return;
            }
            ICollection<Typeface> typefaces = family.GetTypefaces();
            this._lstTypefaces.ItemsSource = typefaces;
            Font font = new(family, 
                            this.SelectedFont.Size.Clamp(8, 
                                                         24), 
                            typefaces.First());
            this.SelectedFont = font;
            this._lstTypefaces.SelectedItem = font.Typeface;
            this._fontSizeSlider.Value = font.Size.Clamp(8, 
                                                         24);
            this._sampleText.DataContext = font;
        }

        private void Typefaces_SelectionChanged(Object sender, 
                                                SelectionChangedEventArgs e)
        {
            Typeface typeface = (Typeface)this._lstTypefaces.SelectedItem;
            if (typeface == this.SelectedFont.Typeface ||
                typeface is null)
            {
                return;
            }
            Font font = new(this.SelectedFont.Family, 
                            this.SelectedFont.Size.Clamp(8, 
                                                         24), 
                            typeface);
            this.SelectedFont = font;
            this._lstFamily.SelectedItem = font.Family;
            this._fontSizeSlider.Value = font.Size.Clamp(8, 
                                                         24);
            this._sampleText.DataContext = font;
        }

        private void SizeSlider_ValueChanged(Object sender, 
                                             RoutedPropertyChangedEventArgs<Double> e)
        {
            if (e.NewValue == this.SelectedFont.Size)
            {
                return;
            }
            Font font = new(this.SelectedFont.Family, 
                            e.NewValue, 
                            this.SelectedFont.Typeface);
            this.SelectedFont = font;
            this._lstFamily.SelectedItem = font.Family;
            this._lstTypefaces.SelectedItem = font.Typeface;
            this._sampleText.DataContext = font;
        }

        private void Ok_Click(Object sender, 
                              RoutedEventArgs e) => 
            this.DialogResult = true;

        private T GetTemplateChild<T>(String childName) 
            where T : DependencyObject => 
                this.GetTemplateChild(childName) is T result 
                    ? result 
                    : throw new InvalidCastException();

        private ListBox _lstFamily;
        private ListBox _lstTypefaces;
        private TextBox _sampleText;
        private Slider _fontSizeSlider;
        private Button _btnOk;
        private Boolean _contentLoaded = false;
    }
}
