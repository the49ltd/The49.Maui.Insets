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

    static void StatusBarStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Page page && page.Window != null)
        {
            UpdateStatusBarStyle(page);
        }
    }

    static void EdgeToEdgeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Page page && page.Window != null)
        {
            UpdateEdgeToEdge(page);
            UpdateStatusBarStyle(page);
        }
    }

    static partial void UpdateEdgeToEdge(Page page);
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
        OnPropertyChanged(nameof(DeviceInsetsThickness));
    }

    internal void SetInsets(Thickness insets)
    {
        _insets = insets;
        OnPropertyChanged(nameof(InsetsThickness));
        OnPropertyChanged(nameof(TopInsetThickness));
        OnPropertyChanged(nameof(BottomInsetThickness));
        OnPropertyChanged(nameof(DeviceInsetsThickness));
    }

    internal void ClearInsets()
    {
        SetInsets(new Thickness(0));
    }

    internal void Init(Page mainPage)
    {
        if (mainPage is Shell shell)
        {
            shell.Navigated += (s, e) =>
            {
                UpdateEdgeToEdge(shell.CurrentPage);
                UpdateStatusBarStyle(shell.CurrentPage);
            };
        }
        else if (mainPage is NavigationPage navigationPage)
        {
            navigationPage.NavigatedTo += (s, e) =>
            {
                UpdateEdgeToEdge(navigationPage.CurrentPage);
                UpdateStatusBarStyle(navigationPage.CurrentPage);
            };
        }
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
}
