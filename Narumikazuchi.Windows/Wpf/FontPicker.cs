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
        ArgumentNullException.ThrowIfNull(owner);

        FontPicker picker = new(owner)
        {
            SelectedFont = Font.Default
        };
        Boolean? result = picker.ShowDialog();
        if (result.HasValue &&
            result.Value)
        {
            return picker.SelectedFont;
        }
        else
        {
            return null;
        }
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
        ArgumentNullException.ThrowIfNull(owner);

        FontPicker picker = new(owner)
        {
            SelectedFont = initial
        };
        Boolean? result = picker.ShowDialog();
        if (result.HasValue &&
            result.Value)
        {
            return picker.SelectedFont;
        }
        else
        {
            return null;
        }
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
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(withStyle);

        FontPicker picker = new(owner)
        {
            SelectedFont = Font.Default,
            Style = withStyle
        };
        Boolean? result = picker.ShowDialog();
        if (result.HasValue &&
            result.Value)
        {
            return picker.SelectedFont;
        }
        else
        {
            return null;
        }
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
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(withStyle);

        FontPicker picker = new(owner)
        {
            SelectedFont = initial,
            Style = withStyle
        };
        Boolean? result = picker.ShowDialog();
        if (result.HasValue &&
            result.Value)
        {
            return picker.SelectedFont;
        }
        else
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (m_LstFamily is not null)
        {
            m_LstFamily.SelectionChanged -= this.Family_SelectionChanged;
        }
        m_LstFamily = this.GetTemplateChild<ListBox>(nameof(m_LstFamily));
        m_LstFamily.SelectionChanged += this.Family_SelectionChanged;

        if (m_LstTypefaces is not null)
        {
            m_LstTypefaces.SelectionChanged -= this.Typefaces_SelectionChanged;
        }
        m_LstTypefaces = this.GetTemplateChild<ListBox>(nameof(m_LstTypefaces));
        m_LstTypefaces.SelectionChanged += this.Typefaces_SelectionChanged;

        m_SampleText = this.GetTemplateChild<TextBox>(nameof(m_SampleText));

        if (m_FontSizeSlider is not null)
        {
            m_FontSizeSlider.ValueChanged -= this.SizeSlider_ValueChanged;
        }
        m_FontSizeSlider = this.GetTemplateChild<Slider>(nameof(m_FontSizeSlider));
        m_FontSizeSlider.ValueChanged += this.SizeSlider_ValueChanged;

        if (m_BtnOk is not null)
        {
            m_BtnOk.Click -= this.Ok_Click;
        }
        m_BtnOk = this.GetTemplateChild<Button>(nameof(m_BtnOk));
        m_BtnOk.Click += this.Ok_Click;

        m_LstFamily.ItemsSource = Fonts.SystemFontFamilies;
        m_LstTypefaces.ItemsSource = this.SelectedFont.Family.GetTypefaces();
        m_LstFamily.SelectedItem = this.SelectedFont.Family;
        m_LstFamily.ScrollIntoView(m_LstFamily.SelectedItem);
        m_LstTypefaces.SelectedItem = this.SelectedFont.Typeface;
        m_FontSizeSlider.Value = this.SelectedFont.Size;
        m_SampleText.DataContext = this.SelectedFont;
    }

    /// <summary>
    /// Identifies the <see cref="SelectedFont"/> property.
    /// </summary>
    [NotNull]
    public static readonly DependencyProperty SelectedFontProperty =
        DependencyProperty.Register(name: nameof(SelectedFont),
                                    propertyType: typeof(Font),
                                    ownerType: typeof(FontPicker),
                                    typeMetadata: new PropertyMetadata(Font.Default));

    /// <summary>
    /// Gets or sets the <see cref="Font"/> that this picker has currently selected.
    /// </summary>
    public Font SelectedFont
    {
        get => (Font)this.GetValue(SelectedFontProperty);
        set => this.SetValue(dp: SelectedFontProperty,
                             value: value);
    }
}

// Non-Public
partial class FontPicker
{
#pragma warning disable CS8618
    private FontPicker(Window owner)
    {
        ArgumentNullException.ThrowIfNull(owner);

        this.Owner = owner;
        this.InitializeComponent();
    }
#pragma warning restore

    private void InitializeComponent()
    {
        if (m_ContentLoaded)
        {
            return;
        }

        m_ContentLoaded = true;
        this.Width = 592;
        this.Height = 380;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.ResizeMode = ResizeMode.NoResize;
        this.WindowStyle = WindowStyle.None;
        this.ShowInTaskbar = false;
        ParserContext context = new();
        context.XmlnsDictionary.Add(prefix: "",
                                    xmlNamespace: "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
        context.XmlnsDictionary.Add(prefix: "x",
                                    xmlNamespace: "http://schemas.microsoft.com/winfx/2006/xaml");
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
                            "<ListBox x:Name=\"" + nameof(m_LstFamily) + "\" Grid.Column=\"0\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
                                "<ListBox.ItemTemplate>" +
                                    "<DataTemplate>" +
                                        "<TextBlock Text=\"{Binding Source}\" />" +
                                    "</DataTemplate>" +
                                "</ListBox.ItemTemplate>" +
                            "</ListBox>" +
                            "<TextBlock Grid.Column=\"1\" Grid.Row=\"0\" Foreground=\"{TemplateBinding Foreground}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Bottom\" Padding=\"5\" FontStyle=\"Italic\" Text=\"Typeface\"/>" +
                            "<ListBox x:Name=\"" + nameof(m_LstTypefaces) + "\" Grid.Column=\"1\" Grid.Row=\"1\" Margin=\"5\" Background=\"{TemplateBinding Background}\" TextElement.Foreground=\"{TemplateBinding Foreground}\">" +
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
                                "<TextBox x:Name=\"" + nameof(m_SampleText) + "\" Grid.Row=\"0\" Background=\"{TemplateBinding Background}\" Foreground=\"{TemplateBinding Foreground}\" IsReadOnly=\"True\" Text=\"Lorem ipsum dolor sit amet, consectetur adipisicing elit\" TextAlignment=\"Center\" TextWrapping=\"Wrap\" FontSize=\"{Binding Size}\" FontFamily=\"{Binding Family}\" FontStretch=\"{Binding Stretch}\" FontStyle=\"{Binding Style}\" FontWeight=\"{Binding Weight}\"/>" +
                                "<Slider x:Name=\"" + nameof(m_FontSizeSlider) + "\" Grid.Row=\"1\" Minimum=\"8\" Maximum=\"24\" Value=\"12\" SmallChange=\"0.5\" LargeChange=\"2\" TickPlacement=\"BottomRight\" AutoToolTipPlacement=\"TopLeft\"/>" +
                            "</Grid>" +
                        "</Grid>" +
                        "<StackPanel Grid.Row=\"1\" HorizontalAlignment=\"Right\" Orientation=\"Horizontal\">" +
                            "<Button x:Name=\"" + nameof(m_BtnOk) + "\" Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsDefault=\"True\" Content=\"OK\"/>" +
                            "<Button Margin=\"4 8\" MinWidth=\"32\" MinHeight=\"24\" IsCancel=\"True\" Content=\"Cancel\"/>" +
                        "</StackPanel>" +
                    "</Grid>" +
                "</Border>" +
            "</ControlTemplate>";
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(xaml));
        ControlTemplate template = (ControlTemplate)XamlReader.Load(stream: stream,
                                                                    parserContext: context);
        this.Template = template;
    }

    private void Family_SelectionChanged(Object sender,
                                         SelectionChangedEventArgs eventArgs)
    {
        FontFamily family = (FontFamily)m_LstFamily.SelectedItem;
        if (family == this.SelectedFont.Family ||
            family is null)
        {
            return;
        }
        ICollection<Typeface> typefaces = family.GetTypefaces();
        m_LstTypefaces.ItemsSource = typefaces;
        Font font = new(family: family,
                        size: this.SelectedFont.Size.Clamp(8, 24),
                        typeface: typefaces.First());
        this.SelectedFont = font;
        m_LstTypefaces.SelectedItem = font.Typeface;
        m_FontSizeSlider.Value = font.Size;
        m_SampleText.DataContext = font;
    }

    private void Typefaces_SelectionChanged(Object sender,
                                            SelectionChangedEventArgs eventArgs)
    {
        Typeface typeface = (Typeface)m_LstTypefaces.SelectedItem;
        if (typeface == this.SelectedFont.Typeface ||
            typeface is null)
        {
            return;
        }
        Font font = new(family: this.SelectedFont.Family,
                        size: this.SelectedFont.Size.Clamp(8, 24),
                        typeface: typeface);
        this.SelectedFont = font;
        m_LstFamily.SelectedItem = font.Family;
        m_FontSizeSlider.Value = font.Size;
        m_SampleText.DataContext = font;
    }

    private void SizeSlider_ValueChanged(Object sender,
                                         RoutedPropertyChangedEventArgs<Double> eventArgs)
    {
        if (eventArgs.NewValue == this.SelectedFont.Size)
        {
            return;
        }
        Font font = new(family: this.SelectedFont.Family,
                        size: eventArgs.NewValue,
                        typeface: this.SelectedFont.Typeface);
        this.SelectedFont = font;
        m_LstFamily.SelectedItem = font.Family;
        m_LstTypefaces.SelectedItem = font.Typeface;
        m_SampleText.DataContext = font;
    }

    private void Ok_Click(Object sender,
                          RoutedEventArgs eventArgs) =>
        this.DialogResult = true;

    private T GetTemplateChild<T>(String childName)
        where T : DependencyObject
    {
        if (this.GetTemplateChild(childName) is T result)
        {
            return result;
        }
        else
        {
            throw new InvalidCastException();
        }
    }

    private ListBox m_LstFamily;
    private ListBox m_LstTypefaces;
    private TextBox m_SampleText;
    private Slider m_FontSizeSlider;
    private Button m_BtnOk;
    private Boolean m_ContentLoaded = false;
}