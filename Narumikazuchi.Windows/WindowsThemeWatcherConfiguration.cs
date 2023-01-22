using Narumikazuchi.Services;

namespace Narumikazuchi.Windows;

/// <summary>
/// Contains the configuration for a <see cref="WindowsThemeWatcher"/>.
/// </summary>
public sealed class WindowsThemeWatcherConfiguration : Service
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsThemeWatcherConfiguration"/> class.
    /// </summary>
    /// <param name="resourceDictionary">The <see cref="System.Windows.ResourceDictionary"/> that contains the current theme.</param>
    /// <param name="defaultThemeUri">The uri path to the default theme.</param>
    /// <param name="lightThemeUri">The uri path to the light theme.</param>
    /// <param name="darkThemeUri">The uri path to the dark theme.</param>
    /// <exception cref="ArgumentNullException"/>
    public WindowsThemeWatcherConfiguration(ResourceDictionary resourceDictionary,
                                            Uri defaultThemeUri,
                                            Uri lightThemeUri,
                                            Uri darkThemeUri)
    {
        ArgumentNullException.ThrowIfNull(resourceDictionary);
        ArgumentNullException.ThrowIfNull(defaultThemeUri);
        ArgumentNullException.ThrowIfNull(lightThemeUri);
        ArgumentNullException.ThrowIfNull(darkThemeUri);

        this.ResourceDictionary = resourceDictionary;
        this.DefaultThemeUri = defaultThemeUri;
        this.LightThemeUri = lightThemeUri;
        this.DarkThemeUri = darkThemeUri;
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsThemeWatcherConfiguration"/> class.
    /// </summary>
    public WindowsThemeWatcherConfiguration()
    { }
    
    /// <summary>
    /// Gets the <see cref="System.Windows.ResourceDictionary"/> that contains the current theme.
    /// </summary>
    public required ResourceDictionary ResourceDictionary
    {
        get;
        init;
    }
    
    /// <summary>
    /// Gets the uri path to the default theme.
    /// </summary>
    public required Uri DefaultThemeUri
    {
        get;
        init;
    }
    
    /// <summary>
    /// Gets the uri path to the light theme.
    /// </summary>
    public required Uri LightThemeUri
    {
        get;
        init;
    }
    
    /// <summary>
    /// Gets the uri path to the dark theme.
    /// </summary>
    public required Uri DarkThemeUri
    {
        get;
        init;
    }
#else

    /// <summary>
    /// Gets the <see cref="System.Windows.ResourceDictionary"/> that contains the current theme.
    /// </summary>
    public ResourceDictionary ResourceDictionary
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the uri path to the default theme.
    /// </summary>
    public Uri DefaultThemeUri
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the uri path to the light theme.
    /// </summary>
    public Uri LightThemeUri
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the uri path to the dark theme.
    /// </summary>
    public Uri DarkThemeUri
    {
        get;
        init;
    }
#endif
}