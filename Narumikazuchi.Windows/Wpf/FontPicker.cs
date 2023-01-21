using Button = System.Windows.Controls.Button;
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
    static public Option<Font> Show([DisallowNull] Window owner)
    {
        ArgumentNullException.ThrowIfNull(owner);

        FontPicker picker = new(owner)
        {
            SelectedFont = Font.Default
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
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
    static public Option<Font> Show([DisallowNull] Window owner,
                                    [DisallowNull] Font initial)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(initial);

        FontPicker picker = new(owner)
        {
            SelectedFont = initial
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
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
    static public Option<Font> Show([DisallowNull] Window owner,
                                    [DisallowNull] Style withStyle)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(withStyle);

        FontPicker picker = new(owner)
        {
            SelectedFont = Font.Default,
            Style = withStyle
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
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
    static public Option<Font> Show([DisallowNull] Window owner,
                                    [DisallowNull] Font initial,
                                    [DisallowNull] Style withStyle)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(initial);
        ArgumentNullException.ThrowIfNull(withStyle);

        FontPicker picker = new(owner)
        {
            SelectedFont = initial,
            Style = withStyle
        };
        Option<Boolean?> result = picker.ShowDialog();
        if (result.HasValue &&
            result.Map(value => value is true))
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
        get
        {
            return (Font)this.GetValue(SelectedFontProperty);
        }
        set
        {
            this.SetValue(dp: SelectedFontProperty,
                                     value: value);
        }
    }
}