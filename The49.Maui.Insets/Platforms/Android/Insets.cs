using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using Microsoft.Maui.Controls.Platform.Compatibility;
using AWindow = Android.Views.Window;

namespace The49.Maui.Insets;

public class InsetsOnApplyWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
{
    public WindowInsetsCompat OnApplyWindowInsets(Android.Views.View v, WindowInsetsCompat windowInsets)
    {
        Insets.Current.SetInsets(windowInsets.ToThickness());
        return ViewCompat.OnApplyWindowInsets(v, windowInsets);
    }
}

public static class WindowInsetsExtensions
{
    public static Thickness ToThickness(this WindowInsetsCompat windowInsets)
    {
        var insets = windowInsets.GetInsets(WindowInsetsCompat.Type.SystemBars());
        return new Thickness(0, insets.Top / DeviceDisplay.MainDisplayInfo.Density, 0, insets.Bottom / DeviceDisplay.MainDisplayInfo.Density);
    }
}

public partial class Insets
{
    static InsetsOnApplyWindowInsetsListener _listener = new InsetsOnApplyWindowInsetsListener();
    internal void InitActivity(Activity activity)
    {
        ViewCompat.SetOnApplyWindowInsetsListener(activity.Window.DecorView, _listener);
    }
    static partial void UpdateEdgeToEdge(Page page)
    {
        var activity = Platform.CurrentActivity as AppCompatActivity;
        var edgeToEdge = GetEdgeToEdge(page);

        if (edgeToEdge)
        {
            activity.Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Current.SetEnabled(true);
        }
        else
        {
            activity.Window.ClearFlags(WindowManagerFlags.LayoutNoLimits);
            Current.SetEnabled(false);
        }
        WindowCompat.SetDecorFitsSystemWindows(activity.Window, !edgeToEdge);


        // Some parts of Shell cancel the fitsSystemWindows and prevent this from working
        // Adding this makes sure it resets this
        // Another way would be to consume the insets at the top, but a lot of other
        // parts of an Android app can rely on reading the insets
        if (Shell.Current is not null)
        {
            var renderer = Shell.Current.Handler;

            var pv = renderer.PlatformView as ViewGroup;

            for (var i = 0; i < pv.ChildCount; i++)
            {
                if (pv.GetChildAt(i) is CustomFrameLayout cfl)
                {
                    cfl.SetFitsSystemWindows(!edgeToEdge);
                }
            }
        }

    }

    static partial void UpdateStatusBarStyle(Page page)
    {
        var edgeToEdge = GetEdgeToEdge(page);
        if (!IsSupported() || !edgeToEdge)
        {
            return;
        }

        var style = GetStatusBarStyle(page);

        switch (style)
        {
            case StatusBarStyle.DarkContent:
                SetStatusBarAppearance(Platform.CurrentActivity, true);
                break;

            case StatusBarStyle.Default:
            case StatusBarStyle.LightContent:
                SetStatusBarAppearance(Platform.CurrentActivity, false);
                break;

            default:
                throw new NotSupportedException($"{nameof(StatusBarStyle)} {style} is not yet supported on Android");
        }
    }

    static void SetStatusBarAppearance(Activity activity, bool isLightStatusBars)
    {
        var window = GetCurrentWindow(activity);
        var windowController = WindowCompat.GetInsetsController(window, window.DecorView);
        windowController.AppearanceLightStatusBars = isLightStatusBars;
    }

    static AWindow GetCurrentWindow(Activity activity)
    {
        var window = activity.Window ?? throw new InvalidOperationException($"{nameof(activity.Window)} cannot be null");
        window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        return window;
    }

    static bool IsSupported()
    {
        if (OperatingSystem.IsAndroidVersionAtLeast((int)BuildVersionCodes.M))
        {
            return true;
        }

        System.Diagnostics.Debug.WriteLine($"This functionality is not available. Minimum supported API is {(int)BuildVersionCodes.M}");
        return false;
    }
}
