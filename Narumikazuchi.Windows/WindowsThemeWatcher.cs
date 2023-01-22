using Microsoft.Win32;
using Narumikazuchi.Services;

namespace Narumikazuchi.Windows;

/// <summary>
/// Watches for a change in the current windows theme.
/// </summary>
public sealed partial class WindowsThemeWatcher : Service
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsThemeWatcher"/> class.
    /// </summary>
    /// <param name="configuration">The configuration for the themes.</param>
    public WindowsThemeWatcher(WindowsThemeWatcherConfiguration configuration)
    {
        WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
        String query = String.Format(provider: CultureInfo.InvariantCulture,
                                     format: @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                                     arg0: currentUser.User!.Value,
                                     arg1: REGISTRYKEYPATH.Replace(@"\", @"\\"),
                                     arg2: REGISTRYVALUENAME);

        m_Watcher = new(query);
        m_ResourceDictionary = configuration.ResourceDictionary;
        m_ThemePaths.Add(key: WindowsTheme.Default,
                         value: configuration.DefaultThemeUri);
        m_ThemePaths.Add(key: WindowsTheme.Light,
                         value: configuration.LightThemeUri);
        m_ThemePaths.Add(key: WindowsTheme.Dark,
                         value: configuration.DarkThemeUri);

        this.StartThemeWatching();
    }

    /// <summary>
    /// Occurs when the windows theme has been changed.
    /// </summary>
    public event EventHandler<WindowsThemeWatcher, ThemeChangedEventArgs>? WindowsThemeChanged;

    /// <summary>
    /// Gets the currently selected windows theme.
    /// </summary>
    static public WindowsTheme WindowsTheme
    {
        get
        {
            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRYKEYPATH);
                if (key is null)
                {
                    return WindowsTheme.Default;
                }

                Object? registryValueObject = key.GetValue(REGISTRYVALUENAME);
                if (registryValueObject is null)
                {
                    return WindowsTheme.Default;
                }

                Int32 registryValue = (Int32)registryValueObject;

                if (SystemParameters.HighContrast ||
                    registryValue is 0)
                {
                    return WindowsTheme.Dark;
                }
                else
                {
                    return WindowsTheme.Light;
                }
            }
            catch
            {
                return WindowsTheme.Default;
            }
        }
    }
}