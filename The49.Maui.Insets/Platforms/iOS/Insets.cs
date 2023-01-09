using Foundation;
using UIKit;

namespace The49.Maui.Insets;

public partial class Insets
{
    static partial void PlatformInit(Page page)
    {
        var insets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
        Current.SetInsets(new Thickness(insets.Left, insets.Top, insets.Right, insets.Bottom));
    }
    static partial void UpdateEdgeToEdge(Page page)
    {
        var useFullWindow = GetEdgeToEdge(page);

        if (useFullWindow)
        {
            Current.SetEnabled(true);
            Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, false);
            page.SetBinding(Page.PaddingProperty, new Binding(nameof(Insets.InsetsThickness), source: Current));
        }
        else
        {
            Current.SetEnabled(false);
            Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, true);
            page.RemoveBinding(Page.PaddingProperty);
        }
    }

    static partial void UpdateStatusBarStyle(Page page)
    {
        var style = GetStatusBarStyle(page);
        var useFullWindow = GetEdgeToEdge(page);

        if (!useFullWindow)
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
        }
        else
        {
            UIApplication.SharedApplication.SetStatusBarStyle(style switch
            {
                StatusBarStyle.LightContent => UIStatusBarStyle.LightContent,
                StatusBarStyle.DarkContent => UIStatusBarStyle.DarkContent,
                StatusBarStyle.Default or _ => UIStatusBarStyle.Default,
            }, false);
        }
        UpdateStatusBarAppearance();
    }
    static void UpdateStatusBarAppearance()
    {
        if (OperatingSystem.IsIOSVersionAtLeast(13))
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                UpdateStatusBarAppearance(window);
            }
        }
        else
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            UpdateStatusBarAppearance(window);
        }
    }

    static void UpdateStatusBarAppearance(UIWindow? window)
    {
        var vc = window?.RootViewController ?? WindowStateManager.Default.GetCurrentUIViewController() ?? throw new InvalidOperationException($"{nameof(window.RootViewController)} cannot be null");

        while (vc.PresentedViewController is not null)
        {
            vc = vc.PresentedViewController;
        }

        vc.SetNeedsStatusBarAppearanceUpdate();
    }
}
