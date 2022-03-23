using Button = System.Windows.Controls.Button;
using FontFamily = System.Windows.Media.FontFamily;
using ListBox = System.Windows.Controls.ListBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Narumikazuchi.Windows.Wpf;

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

        if (this.m_LstFamily is not null)
        {
            this.m_LstFamily.SelectionChanged -= this.Family_SelectionChanged;
        }
        this.m_LstFamily = this.GetTemplateChild<ListBox>(nameof(this.m_LstFamily));
        this.m_LstFamily.SelectionChanged += this.Family_SelectionChanged;

        if (this.m_LstTypefaces is not null)
        {
            this.m_LstTypefaces.SelectionChanged -= this.Typefaces_SelectionChanged;
        }
        this.m_LstTypefaces = this.GetTemplateChild<ListBox>(nameof(this.m_LstTypefaces));
        this.m_LstTypefaces.SelectionChanged += this.Typefaces_SelectionChanged;

        this.m_SampleText = this.GetTemplateChild<TextBox>(nameof(this.m_SampleText));

        if (this.m_FontSizeSlider is not null)
        {
            this.m_FontSizeSlider.ValueChanged -= this.SizeSlider_ValueChanged;
        }
        this.m_FontSizeSlider = this.GetTemplateChild<Slider>(nameof(this.m_FontSizeSlider));
        this.m_FontSizeSlider.ValueChanged += this.SizeSlider_ValueChanged;

        if (this.m_BtnOk is not null)
        {
            this.m_BtnOk.Click -= this.Ok_Click;
        }
        this.m_BtnOk = this.GetTemplateChild<Button>(nameof(this.m_BtnOk));
        this.m_BtnOk.Click += this.Ok_Click;

        this.m_LstFamily.ItemsSource = Fonts.SystemFontFamilies;
        this.m_LstTypefaces.ItemsSource = this.SelectedFont.Family.GetTypefaces();
        this.m_LstFamily.SelectedItem = this.SelectedFont.Family;
        this.m_LstFamily.ScrollIntoView(this.m_LstFamily.SelectedItem);
        this.m_LstTypefaces.SelectedItem = this.SelectedFont.Typeface;
        this.m_FontSizeSlider.Value = this.SelectedFont.Size;
        this.m_SampleText.DataContext = this.SelectedFont;
    }

    /// <summary>
    /// Identifies the <see cref="SelectedFont"/> property.
    /// </summary>
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
        if (this.m_ContentLoaded)
        {
            return;
        }

        this.m_ContentLoaded = true;
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
                            "<ListBox x:Name=\"" + nameof(this.m_LstFamily) + "\" Grid.Column=\"0\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                                "<ListBox.ItemTemplate>" +
                                    "<DataTemplate>" +
                                        "<TextBlock Text=\"{Binding Source}\" />" +
                                    "</DataTemplate>" +
                                "</ListBox.ItemTemplate>" +
                            "</ListBox>" +
                            "<TextBlock Grid.Column=\"1\" Grid.Row=\"0\" Foreground=\"{TemplateBinding Foreground}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Bottom\" Padding=\"5\" FontStyle=\"Italic\" Text=\"Typeface\"/>" +
                            "<ListBox x:Name=\"" + nameof(this.m_LstTypefaces) + "\" Grid.Column=\"1\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
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
                                "<TextBox x:Name=\"" + nameof(this.m_SampleText) + "\" Grid.Row=\"0\" Background=\"{TemplateBinding Background}\" Foreground=\"{TemplateBinding Foreground}\" IsReadOnly=\"True\" Text=\"Lorem ipsum dolor sit amet, consectetur adipisicing elit\" TextAlignment=\"Center\" TextWrapping=\"Wrap\" FontSize=\"{Binding Size}\" FontFamily=\"{Binding Family}\" FontStretch=\"{Binding Stretch}\" FontStyle=\"{Binding Style}\" FontWeight=\"{Binding Weight}\"/>" +
                                "<Slider x:Name=\"" + nameof(this.m_FontSizeSlider) + "\" Grid.Row=\"1\" Minimum=\"8\" Maximum=\"24\" Value=\"12\" SmallChange=\"0.5\" LargeChange=\"2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\"/>" +
                            "</Grid>" +
                        "</Grid>" +
                        "<StackPanel Grid.Row=\"1\" HorizontalAlignment=\"Right\" Orientation=\"Horizontal\">" +
                            "<Button x:Name=\"" + nameof(this.m_BtnOk) + "\" Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsDefault=\"True\" Content=\"OK\"/>" +
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
        FontFamily family = (FontFamily)this.m_LstFamily.SelectedItem;
        if (family == this.SelectedFont.Family ||
            family is null)
        {
            return;
        }
        ICollection<Typeface> typefaces = family.GetTypefaces();
        this.m_LstTypefaces.ItemsSource = typefaces;
        Font font = new(family,
                        this.SelectedFont.Size.Clamp(8,
                                                     24),
                        typefaces.First());
        this.SelectedFont = font;
        this.m_LstTypefaces.SelectedItem = font.Typeface;
        this.m_FontSizeSlider.Value = font.Size.Clamp(8,
                                                     24);
        this.m_SampleText.DataContext = font;
    }

    private void Typefaces_SelectionChanged(Object sender,
                                            SelectionChangedEventArgs e)
    {
        Typeface typeface = (Typeface)this.m_LstTypefaces.SelectedItem;
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
        this.m_LstFamily.SelectedItem = font.Family;
        this.m_FontSizeSlider.Value = font.Size.Clamp(8,
                                                     24);
        this.m_SampleText.DataContext = font;
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
        this.m_LstFamily.SelectedItem = font.Family;
        this.m_LstTypefaces.SelectedItem = font.Typeface;
        this.m_SampleText.DataContext = font;
    }

    private void Ok_Click(Object sender,
                          RoutedEventArgs e) =>
        this.DialogResult = true;

    private T GetTemplateChild<T>(String childName)
        where T : DependencyObject =>
            this.GetTemplateChild(childName) is T result
                ? result
                : throw new InvalidCastException();

    private ListBox m_LstFamily;
    private ListBox m_LstTypefaces;
    private TextBox m_SampleText;
    private Slider m_FontSizeSlider;
    private Button m_BtnOk;
    private Boolean m_ContentLoaded = false;
}