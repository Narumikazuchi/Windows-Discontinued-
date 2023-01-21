using Button = System.Windows.Controls.Button;
using FontFamily = System.Windows.Media.FontFamily;
using ListBox = System.Windows.Controls.ListBox;
using TextBox = System.Windows.Controls.TextBox;

namespace Narumikazuchi.Windows.Wpf;

public partial class FontPicker
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
        else
        {
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
        else
        {
            Typeface[] typefaces = family.GetTypefaces()
                                         .ToArray();
            m_LstTypefaces.ItemsSource = typefaces;
            Font font = new(family: family,
                            size: this.SelectedFont.Size.Clamp(8, 24),
                            typeface: typefaces.First());
            this.SelectedFont = font;
            m_LstTypefaces.SelectedItem = font.Typeface;
            m_FontSizeSlider.Value = font.Size;
            m_SampleText.DataContext = font;
        }
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
        else
        {
            Font font = new(family: this.SelectedFont.Family,
                            size: this.SelectedFont.Size.Clamp(8, 24),
                            typeface: typeface);
            this.SelectedFont = font;
            m_LstFamily.SelectedItem = font.Family;
            m_FontSizeSlider.Value = font.Size;
            m_SampleText.DataContext = font;
        }
    }

    private void SizeSlider_ValueChanged(Object sender,
                                         RoutedPropertyChangedEventArgs<Double> eventArgs)
    {
        if (eventArgs.NewValue == this.SelectedFont.Size)
        {
            return;
        }
        else
        {
            Font font = new(family: this.SelectedFont.Family,
                            size: eventArgs.NewValue,
                            typeface: this.SelectedFont.Typeface);
            this.SelectedFont = font;
            m_LstFamily.SelectedItem = font.Family;
            m_LstTypefaces.SelectedItem = font.Typeface;
            m_SampleText.DataContext = font;
        }
    }

    private void Ok_Click(Object sender,
                          RoutedEventArgs eventArgs)
    {
        this.DialogResult = true;
    }

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