using Narumikazuchi.Windows.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Narumikazuchi.Windows
{
    /// <summary>
    /// Fixes the maximize issue with <see cref="WindowStyle.None"/> covering the taskbar.
    /// </summary>
    public sealed partial class BorderlessWindowResizer
    {
#pragma warning disable
        /// <summary>
        /// Attaches a new instance of <see cref="BorderlessWindowResizer"/> to the <see cref="Window"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void AttachTo([DisallowNull] Window window) => 
            _attached.Add(window, 
                          new(window));
#pragma warning restore

        /// <summary>
        /// Gets the <see cref="BorderlessWindowResizer"/> attached to the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> to look up.</param>
        /// <returns>The <see cref="BorderlessWindowResizer"/> attached to the specified window or <see langword="null"/>, if there is none attached</returns>
        [Pure]
        [return: MaybeNull]
        public static BorderlessWindowResizer? GetResizerForWindow([DisallowNull] Window window)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }
            if (!_attached.ContainsKey(window))
            {
                return null;
            }
            return _attached[window];
        }

        /// <summary>
        /// Occurs when the dock of the <see cref="Window"/> changed.
        /// </summary>
        public event EventHandler<BorderlessWindowResizer, WindowDockEventArgs>? WindowDockChanged;
    }

    // Non-Public
    partial class BorderlessWindowResizer
    {
        private BorderlessWindowResizer(Window window)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            this._window = window;

            this.GetTransfrom();

            this._window.SourceInitialized += this.Window_SourceInitialized;
            this._window.SizeChanged += this.Window_SizeChanged;
        }

        private void GetTransfrom()
        {
            PresentationSource source = PresentationSource.FromVisual(this._window);
            if (source is null)
            {
                return;
            }

            this._transform = source.CompositionTarget.TransformToDevice;
        }

        private IntPtr WindowProc(IntPtr hwnd, 
                                  Int32 msg, 
                                  IntPtr wParam, 
                                  IntPtr lParam, 
                                  ref Boolean handled)
        {
            // WM_GETMINMAXINFO
            if (msg == 0x0024)
            {
                this.WmGetMinMaxInfo(lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(in IntPtr lParam)
        {
#nullable disable
            GetCursorPos(out __Point lpMouseLocation);
            IntPtr lpPrimaryScreen = MonitorFromPoint(new __Point(0, 
                                                                  0), 
                                                      __MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            __MonitorInfo primaryScreenInfo = new();
            if (!GetMonitorInfo(lpPrimaryScreen, 
                                primaryScreenInfo))
            {
                return;
            }

            IntPtr lpCurrentScreen = MonitorFromPoint(lpMouseLocation, 
                                                      __MonitorOptions.MONITOR_DEFAULTTONEAREST);
            if (lpCurrentScreen != this._lastScreen ||
                this._transform == default)
            {
                this.GetTransfrom();
            }
            this._lastScreen = lpCurrentScreen;

            __MinMaxInfo info = (__MinMaxInfo)Marshal.PtrToStructure(lParam, 
                                                                     typeof(__MinMaxInfo));
            if (lpPrimaryScreen == lpCurrentScreen)
            {
                info.ptMaxPosition.X = primaryScreenInfo.rcWork.Left;
                info.ptMaxPosition.Y = primaryScreenInfo.rcWork.Top;
                info.ptMaxSize.X = primaryScreenInfo.rcWork.Right - primaryScreenInfo.rcWork.Left;
                info.ptMaxSize.Y = primaryScreenInfo.rcWork.Bottom - primaryScreenInfo.rcWork.Top;
            }
            else
            {
                info.ptMaxPosition.X = primaryScreenInfo.rcMonitor.Left;
                info.ptMaxPosition.Y = primaryScreenInfo.rcMonitor.Top;
                info.ptMaxSize.X = primaryScreenInfo.rcMonitor.Right - primaryScreenInfo.rcMonitor.Left;
                info.ptMaxSize.Y = primaryScreenInfo.rcMonitor.Bottom - primaryScreenInfo.rcMonitor.Top;
            }

            Point minSize = this._transform.Transform(new Point(this._window.MinWidth, 
                                                                this._window.MinHeight));
            info.ptMinTrackSize.X = (Int32)minSize.X;
            info.ptMinTrackSize.Y = (Int32)minSize.Y;

            this._screenSize = new Rect(info.ptMaxPosition.X, 
                                        info.ptMaxPosition.Y, 
                                        info.ptMaxSize.X, 
                                        info.ptMaxSize.Y);
            Marshal.StructureToPtr(info, 
                                   lParam, 
                                   true);
#nullable enable
        }

        private void Window_SourceInitialized(Object? sender, 
                                              EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this._window).Handle;
            HwndSource source = HwndSource.FromHwnd(handle);
            if (source is null)
            {
                return;
            }

            source.AddHook(this.WindowProc);
        }

        private void Window_SizeChanged(Object? sender, 
                                        SizeChangedEventArgs e)
        {
            if (this._transform == default)
            {
                return;
            }

            Size size = e.NewSize;

            Double left = this._window.Left;
            Double top = this._window.Top;
            Double right = left + this._window.Width;
            Double bottom = top + this._window.Height;

            Point topLeft = this._transform.Transform(new Point(left, 
                                                                top));
            Point bottomRight = this._transform.Transform(new Point(right, 
                                                                    bottom));

            Boolean edgedLeft = topLeft.X <= this._screenSize.Left + EDGETOLERANCE;
            Boolean edgedTop = topLeft.Y <= this._screenSize.Top + EDGETOLERANCE;
            Boolean edgedRight = bottomRight.X >= this._screenSize.Right - EDGETOLERANCE;
            Boolean edgedBottom = bottomRight.Y >= this._screenSize.Bottom - EDGETOLERANCE;

            WindowDockPosition dock = WindowDockPosition.Undocked;
            if (edgedTop &&
                edgedBottom && 
                edgedLeft)
            {
                dock = WindowDockPosition.Left;
            }
            else if (edgedTop &&
                     edgedBottom &&
                     edgedRight)
            {
                dock = WindowDockPosition.Right;
            }

            if (dock != this._lastDock)
            {
                this.WindowDockChanged?.Invoke(this,
                                               new(dock));
                this._lastDock = dock;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetCursorPos(out __Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetMonitorInfo(IntPtr hMonitor, 
                                                     __MonitorInfo lpmi);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(__Point pt, 
                                                      __MonitorOptions dwFlags);

        private static Dictionary<Window, BorderlessWindowResizer> _attached = new();
        private readonly Window _window;
        private WindowDockPosition _lastDock = WindowDockPosition.Undocked;
        private IntPtr _lastScreen = IntPtr.Zero;
        private Rect _screenSize = new();
        private Matrix _transform = default;

        private const Int32 EDGETOLERANCE = 2;
    }
}
