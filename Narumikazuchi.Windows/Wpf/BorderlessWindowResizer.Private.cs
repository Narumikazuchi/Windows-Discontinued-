using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Narumikazuchi.Windows.Wpf;

public partial class BorderlessWindowResizer
{
    private BorderlessWindowResizer(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        m_Window = window;
        m_LastState = window.WindowState;

        this.GetTransfrom();

        m_Window.SourceInitialized += this.Window_SourceInitialized;
        m_Window.SizeChanged += this.Window_SizeChanged;
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

    private void GetTransfrom()
    {
        PresentationSource source = PresentationSource.FromVisual(m_Window);
        if (source is null)
        {
            return;
        }
        else
        {
            m_Transform = source.CompositionTarget.TransformToDevice;
        }
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
        GetCursorPos(out __Point lpMouseLocation);
        IntPtr lpPrimaryScreen = MonitorFromPoint(new __Point(0, 0),
                                                  __MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
        __MonitorInfo primaryScreenInfo = new();
        if (!GetMonitorInfo(lpPrimaryScreen,
                            primaryScreenInfo))
        {
            return;
        }

        IntPtr lpCurrentScreen = MonitorFromPoint(lpMouseLocation,
                                                  __MonitorOptions.MONITOR_DEFAULTTONEAREST);
        if (lpCurrentScreen != m_LastScreen ||
            m_Transform == default)
        {
            this.GetTransfrom();
        }
        m_LastScreen = lpCurrentScreen;

        __MinMaxInfo info = (__MinMaxInfo)Marshal.PtrToStructure(lParam,
                                                                 typeof(__MinMaxInfo))!;
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

        Point minSize = m_Transform.Transform(new Point(m_Window.MinWidth,
                                                        m_Window.MinHeight));
        info.ptMinTrackSize.X = (Int32)minSize.X;
        info.ptMinTrackSize.Y = (Int32)minSize.Y;

        m_ScreenSize = new Rect(info.ptMaxPosition.X,
                                info.ptMaxPosition.Y,
                                info.ptMaxSize.X,
                                info.ptMaxSize.Y);
        Marshal.StructureToPtr(info,
                               lParam,
                               true);
    }

    private void Window_SourceInitialized(Object? sender,
                                          EventArgs e)
    {
        IntPtr handle = new WindowInteropHelper(m_Window).Handle;
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
        if (m_Window.WindowState != m_LastState)
        {
            if (m_LastState is WindowState.Normal &&
                m_Window.WindowState is WindowState.Maximized)
            {
                this.WindowMaximized?
                    .Invoke(sender: m_Window,
                            eventArgs: null);
            }
            else if (m_LastState is WindowState.Maximized &&
                     m_Window.WindowState is WindowState.Normal)
            {
                this.WindowNormalized?
                    .Invoke(sender: m_Window,
                            eventArgs: null);
            }

            m_LastState = m_Window.WindowState;
        }

        if (m_Transform == default)
        {
            return;
        }

        Size size = e.NewSize;

        Double left = m_Window.Left;
        Double top = m_Window.Top;
        Double right = left + m_Window.Width;
        Double bottom = top + m_Window.Height;

        Point topLeft = m_Transform.Transform(new Point(left,
                                                        top));
        Point bottomRight = m_Transform.Transform(new Point(right,
                                                            bottom));

        Boolean edgedLeft = topLeft.X <= m_ScreenSize.Left + EDGETOLERANCE;
        Boolean edgedTop = topLeft.Y <= m_ScreenSize.Top + EDGETOLERANCE;
        Boolean edgedRight = bottomRight.X >= m_ScreenSize.Right - EDGETOLERANCE;
        Boolean edgedBottom = bottomRight.Y >= m_ScreenSize.Bottom - EDGETOLERANCE;

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

        if (dock != m_LastDock)
        {
            this.WindowDockChanged?.Invoke(this,
                                           new(dock));
            m_LastDock = dock;
        }
    }

    static private readonly Dictionary<Window, BorderlessWindowResizer> s_Attached = new();

    private readonly Window m_Window;
    private WindowDockPosition m_LastDock = WindowDockPosition.Undocked;
    private WindowState m_LastState = WindowState.Normal;
    private IntPtr m_LastScreen = IntPtr.Zero;
    private Rect m_ScreenSize = new();
    private Matrix m_Transform = default;

    private const Int32 EDGETOLERANCE = 2;
}