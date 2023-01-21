namespace Narumikazuchi.Windows;

/// <summary>
/// Contains helpers for the windows platform.
/// </summary>
static public partial class Utility
{
    /// <summary>
    /// Frees and disposes the <see cref="IDisposable"/> safely.
    /// </summary>
    /// <param name="disposable">The <see cref="IDisposable"/> to dispose.</param>
    static public void SafeDispose<T>([AllowNull] ref T? disposable)
        where T : IDisposable
    {
        IDisposable? t = disposable;
        disposable = default;

        if (t is not null)
        {
            t.Dispose();
        }
    }

    /// <summary>
    /// Frees and releases COM-Objects.
    /// </summary>
    /// <param name="comObject">The COM-Object to release.</param>
    [SecurityCritical]
    static public void SafeRelease<T>([AllowNull] ref T? comObject)
        where T : class
    {
        T? t = comObject;
        comObject = default;
        if (t is not null)
        {
            if (Marshal.IsComObject(t))
            {
#pragma warning disable
                Marshal.ReleaseComObject(t);
#pragma warning restore
            }
        }
    }

    /// <summary>
    /// Gets the description for the provided exit code.
    /// </summary>
    /// <param name="code">The exit code to get the description of.</param>
    /// <returns>The description for the provided exit code or <see langword="null"/>, if there is no description for the exit code</returns>
    [Pure]
    [return: MaybeNull]
    static public Option<String> GetExitCodeDescription(in Int32 code)
    {
        IntPtr lpMsgBuffer = IntPtr.Zero;
        UInt32 dwFlags = FormatMessage(dwFlags: FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER,
                                       lpSource: IntPtr.Zero,
                                       dwMessageId: (UInt32)code,
                                       dwLanguageId: 0,
                                       lpBuffer: ref lpMsgBuffer,
                                       nSize: 0,
                                       pArguments: IntPtr.Zero);
        if (dwFlags == 0)
        {
            LocalFree(lpMsgBuffer);
            return "Unknown Error.";
        }
        else
        {
            String? szReturn = Marshal.PtrToStringAnsi(lpMsgBuffer);
            LocalFree(lpMsgBuffer);
            return szReturn;
        }
    }

    /// <summary>
    /// Gets if the current version of windows is Windows Vista or newer.
    /// </summary>
    [Pure]
    static public Boolean IsOSVistaOrNewer
    {
        get
        {
            return Environment.OSVersion.Version >= new Version(6, 0);
        }
    }

    /// <summary>
    /// Gets if the current version of windows is Windows 7 or newer.
    /// </summary>
    [Pure]
    static public Boolean IsOSWindows7OrNewer
    {
        get
        {
            return Environment.OSVersion.Version >= new Version(6, 1);
        }
    }

    /// <summary>
    /// Gets if the current version of windows is Windows 8 or newer.
    /// </summary>
    [Pure]
    static public Boolean IsOSWindows8OrNewer
    {
        get
        {
            return Environment.OSVersion.Version >= new Version(6, 2);
        }
    }
}