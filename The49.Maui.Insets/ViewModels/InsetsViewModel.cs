using Microsoft.Maui.Handlers;

namespace The49.Maui.Insets;

public enum StatusBarStyle
{
    Default = 0,
    LightContent = 1,
    DarkContent = 2
}

public partial class Insets : BindableObject
{
    bool _enabled = false;

    public static readonly BindableProperty EdgeToEdgeProperty = BindableProperty.CreateAttached("EdgeToEdge", typeof(bool), typeof(Insets), false, propertyChanged: EdgeToEdgeChanged);
    public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached("StatusBarStyle", typeof(StatusBarStyle), typeof(Insets), StatusBarStyle.Default, propertyChanged: StatusBarStyleChanged);
    public static readonly BindableProperty CancelIOSPaddingProperty = BindableProperty.CreateAttached("CancelIOSPadding", typeof(bool), typeof(Insets), false, propertyChanged: CancelIOSPaddingChanged);

    private static void CancelIOSPaddingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Layout layout)
        {
            UpdateIOSPadding(layout);
        }
    }

    static void StatusBarStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Page page && page.Window != null)
        {
            UpdateStatusBarStyle(page);
        }
    }

    static void EdgeToEdgeChanged(BindableObject bindable, object oldValue, object newValue)
    {

        if (bindable is Page page)
        {
            if (page.Window != null)
            {
                UpdateEdgeToEdge(page);
                UpdateStatusBarStyle(page);

            }
            else
            {
                page.PropertyChanged += PagePropertyChanged;
            }
        }
    }

    private static void PagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var page = (Page)sender;
        if (e.PropertyName == "Window")
        {
            page.PropertyChanged -= PagePropertyChanged;
            if (page.Window.Handler is null)
            {
                void WindowHandlerChanged(object sender, EventArgs e)
                {
                    var window = (Window)sender;
                    window.PropertyChanged -= WindowHandlerChanged;
                    SetupPage(page);
                }
                page.Window.HandlerChanged += WindowHandlerChanged;
            }
            else
            {
                SetupPage(page);
            }
        }
    }

    static void SetupPage(Page page)
    {
        UpdateEdgeToEdge(page);
        UpdateStatusBarStyle(page);
    }

    static partial void UpdateEdgeToEdge(Page page);
    static partial void UpdateIOSPadding(Layout layout);
    static partial void UpdateStatusBarStyle(Page page);
    static partial void PlatformInit(Page page);

    public static Insets Current = new Insets();

    Thickness _insets = new Thickness(0);

    internal void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        OnPropertyChanged(nameof(InsetsThickness));
        OnPropertyChanged(nameof(TopInsetThickness));
        OnPropertyChanged(nameof(BottomInsetThickness));
    }

    internal void SetInsets(Thickness insets)
    {
        _insets = insets;
        OnPropertyChanged(nameof(InsetsThickness));
        OnPropertyChanged(nameof(TopInsetThickness));
        OnPropertyChanged(nameof(BottomInsetThickness));
        OnPropertyChanged(nameof(DeviceInsetsThickness));
    }

    internal void Init(Page mainPage)
    {
        PageHandler.Mapper.PrependToMapping("_", (h, v) =>
        {
            var page = (Page)v;
            if (page.Window is not null)
            {
                UpdateEdgeToEdge(page);
                UpdateStatusBarStyle(page);
            }
            page.Appearing += (s, e) =>
            {
                UpdateEdgeToEdge(page);
                UpdateStatusBarStyle(page);
            };
        });
        PlatformInit(mainPage);
    }

    public Thickness DeviceInsetsThickness => _insets;
    public Thickness InsetsThickness => _enabled ? _insets : new Thickness(0);
    public Thickness TopInsetThickness => _enabled ? new Thickness(0, _insets.Top, 0, 0) : new Thickness(0);
    public Thickness BottomInsetThickness => _enabled ? new Thickness(0, 0, 0, _insets.Bottom) : new Thickness(0);

    public static bool GetEdgeToEdge(BindableObject target)
    {
        return (bool)target.GetValue(EdgeToEdgeProperty);
    }

    public static void SetEdgeToEdge(BindableObject target, bool value)
    {
        target.SetValue(EdgeToEdgeProperty, value);
    }

    public static StatusBarStyle GetStatusBarStyle(BindableObject target)
    {
        return (StatusBarStyle)target.GetValue(StatusBarStyleProperty);
    }

    public static void SetStatusBarStyle(BindableObject target, StatusBarStyle value)
    {
        target.SetValue(StatusBarStyleProperty, value);
    }
    public static bool GetCancelIOSPadding(BindableObject target)
    {
        return (bool)target.GetValue(CancelIOSPaddingProperty);
    }

    public static void SetCancelIOSPadding(BindableObject target, bool value)
    {
        target.SetValue(CancelIOSPaddingProperty, value);
    }
}
