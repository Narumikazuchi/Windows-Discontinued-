using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Contains helpers for the windows platform.
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// Frees and disposes the <see cref="IDisposable"/> safely.
        /// </summary>
        /// <param name="disposable">The <see cref="IDisposable"/> to dispose.</param>
        public static void SafeDispose<T>([MaybeNull] ref T? disposable) 
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
        public static void SafeRelease<T>([MaybeNull] ref T? comObject) 
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
        public static String? GetExitCodeDescription(in Int32 code)
        {
            IntPtr lpMsgBuffer = IntPtr.Zero;
            UInt32 dwFlags = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER, 
                                           IntPtr.Zero, 
                                           (UInt32)code, 
                                           0, 
                                           ref lpMsgBuffer, 
                                           0, 
                                           IntPtr.Zero);
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
        public static Boolean IsOSVistaOrNewer => 
            Environment.OSVersion.Version >= new Version(6, 0);
        /// <summary>
        /// Gets if the current version of windows is Windows 7 or newer.
        /// </summary>
        [Pure]
        public static Boolean IsOSWindows7OrNewer => 
            Environment.OSVersion.Version >= new Version(6, 1);
        /// <summary>
        /// Gets if the current version of windows is Windows 8 or newer.
        /// </summary>
        [Pure]
        public static Boolean IsOSWindows8OrNewer => 
            Environment.OSVersion.Version >= new Version(6, 2);
    }

    // Non-Public
    partial class Utility
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern UInt32 FormatMessage(UInt32 dwFlags, 
                                                   IntPtr lpSource, 
                                                   UInt32 dwMessageId, 
                                                   UInt32 dwLanguageId, 
                                                   ref IntPtr lpBuffer, 
                                                   UInt32 nSize, 
                                                   IntPtr pArguments);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LocalFree(IntPtr Name);

        private const UInt32 FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        private const UInt32 FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        private const UInt32 FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
    }
}
