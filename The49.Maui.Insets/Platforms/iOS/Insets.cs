using System.Runtime.CompilerServices;
using UIKit;

namespace The49.Maui.Insets;

public partial class Insets
{
    public Thickness NegativeInsetsThickness => new Thickness(-InsetsThickness.Left, -InsetsThickness.Top, -InsetsThickness.Right, -InsetsThickness.Bottom);

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(InsetsThickness))
        {
            OnPropertyChanged(nameof(NegativeInsetsThickness));
        }
    }
    static partial void PlatformInit(Page page)
    {
        var insets = UIApplication.SharedApplication.GetSafeAreaInsetsForWindow();
        Current.SetInsets(new Thickness(insets.Left, insets.Top, insets.Right, insets.Bottom));
    }
    static partial void UpdateIOSPadding(Layout layout)
    {
        if (GetCancelIOSPadding(layout))
        {
            layout.SetBinding(Layout.PaddingProperty, new Binding(nameof(NegativeInsetsThickness), source: Current));
        }
        else
        {
            layout.RemoveBinding(Layout.PaddingProperty);
        }
    }
    static partial void UpdateEdgeToEdge(Page page)
    {
        var edgeToEdge = GetEdgeToEdge(page);

        var runtimeVersion = Environment.Version;
        if (edgeToEdge)
        {
            Current.SetEnabled(true);
            if (runtimeVersion.Major < 8)
            {
                page.SetBinding(Page.PaddingProperty, new Binding(nameof(NegativeInsetsThickness), source: Current));
            }
        }
        else
        {
            Current.SetEnabled(false);
            if (runtimeVersion.Major < 8)
            {
                page.RemoveBinding(Page.PaddingProperty);
            }
        }
    }

    static partial void UpdateStatusBarStyle(Page page)
    {
        var style = GetStatusBarStyle(page);
        var edgeToEdge = GetEdgeToEdge(page);

        if (!edgeToEdge)
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
