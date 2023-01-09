//using Microsoft.UI;
//using Microsoft.UI.Windowing;
//using WWindow = Microsoft.UI.Xaml.Window;

namespace The49.Maui.Insets;

// TODO: The code here is partially working, needs more work to be more reliable
public partial class Insets
{
    //static partial void UpdateEdgeToEdge(Page page)
    //{
    //    var useFullWindow = GetEdgeToEdge(page);

    //    var window = page.Window;

    //    var wWindow = (WWindow)window.Handler.PlatformView;

    //    var appWindow = GetAppWindowForCurrentWindow(wWindow);

    //    if (useFullWindow)
    //    {
    //        Current.SetInsets(new Thickness(0, 32, 0, 0));
    //        Current.SetEnabled(true);
    //        appWindow.TitleBar.BackgroundColor = Microsoft.UI.Colors.Transparent;
    //    }
    //    else
    //    {
    //        Current.SetEnabled(false);
    //        appWindow.TitleBar.BackgroundColor = null;
    //    }
    //    wWindow.ExtendsContentIntoTitleBar = useFullWindow;
    //    appWindow.TitleBar.ExtendsContentIntoTitleBar = useFullWindow;

    //    // TODO: customise app title bar with DataTemplate
    //    // TODO: provide a decent default data template that handles statusbar style
    //}

    //static partial void UpdateStatusBarStyle(Page page)
    //{
    //    var style = GetStatusBarStyle(page);
    //    var useFullWindow = GetEdgeToEdge(page);

    //    var window = page.Window;

    //    var wWindow = (WWindow)window.Handler.PlatformView;
    //    var appWindow = GetAppWindowForCurrentWindow(wWindow);
    //    var titleBar = appWindow.TitleBar;

    //    if (!useFullWindow)
    //    {
    //        titleBar.ButtonBackgroundColor = null;
    //        titleBar.ButtonInactiveBackgroundColor = null;
    //        titleBar.ButtonForegroundColor = null;
    //        titleBar.ButtonInactiveForegroundColor = null;
    //    }
    //    else
    //    {
    //        titleBar.ButtonBackgroundColor = Microsoft.UI.Colors.Transparent;
    //        titleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Transparent;
    //        if (style == StatusBarStyle.LightContent)
    //        {
    //            titleBar.ButtonForegroundColor = Microsoft.UI.Colors.White;
    //            titleBar.ButtonInactiveForegroundColor = Microsoft.UI.Colors.White;
    //        }
    //        else
    //        {
    //            titleBar.ButtonForegroundColor = Microsoft.UI.Colors.Black;
    //            titleBar.ButtonInactiveForegroundColor = Microsoft.UI.Colors.Black;
    //        }
    //    }
    //}
    //public static AppWindow GetAppWindowForCurrentWindow(object target)
    //{
    //    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(target);
    //    WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
    //    return AppWindow.GetFromWindowId(wndId);
    //}
}
