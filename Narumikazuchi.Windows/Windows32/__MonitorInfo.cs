using System;
using System.Runtime.InteropServices;

namespace Narumikazuchi.Windows.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class __MonitorInfo
    {
        #region Properties

        public Int32 cbSize = Marshal.SizeOf(typeof(__MonitorInfo));
        public __Rectangle rcMonitor = new __Rectangle();
        public __Rectangle rcWork = new __Rectangle();
        public Int32 dwFlags = 0;

        #endregion
    }
}
