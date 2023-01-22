namespace Narumikazuchi.Windows;

public partial class WindowsThemeWatcher
{
    private void StartThemeWatching()
    {
        try
        {
            this.MergeThemeDictionaries(WindowsTheme);
            m_Watcher.EventArrived += this.Watcher_EventArrived;
            SystemParameters.StaticPropertyChanged += this.SystemParameters_PropertyChanged;
            // Start listening for events
            m_Watcher.Start();
        }
        // This can fail on Windows 7
        catch
        { }
    }

    private void MergeThemeDictionaries(WindowsTheme windowsTheme)
    {
        m_ResourceDictionary.Source = m_ThemePaths[windowsTheme];
    }

    private void SystemParameters_PropertyChanged(Object? sender,
                                                  PropertyChangedEventArgs e)
    {
        this.MergeThemeDictionaries(WindowsTheme);

        this.WindowsThemeChanged?.Invoke(sender: this,
                                         eventArgs: new(WindowsTheme));
    }

    private void Watcher_EventArrived(Object sender,
                                      EventArrivedEventArgs e)
    {
        this.MergeThemeDictionaries(WindowsTheme);

        this.WindowsThemeChanged?.Invoke(sender: this,
                                         eventArgs: new(WindowsTheme));
    }

    private readonly Dictionary<WindowsTheme, Uri> m_ThemePaths = new();
    private readonly ResourceDictionary m_ResourceDictionary;
    private readonly ManagementEventWatcher m_Watcher;

    private const String REGISTRYKEYPATH = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const String REGISTRYVALUENAME = "AppsUseLightTheme";
}