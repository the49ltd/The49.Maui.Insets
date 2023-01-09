namespace The49.Maui.Insets.Sample;

public partial class MainPage : ContentPage
{
    bool _shouldUseLightStyle = true;
    bool _shouldUseEdgeToEdge = false;

    public MainPage()
    {
        InitializeComponent();
        Insets.Current.PropertyChanged += Insets_PropertyChanged;
    }

    void Insets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SimulatedDeviceInsets));
    }

    public StatusBarStyle StatusBarStyle => _shouldUseLightStyle ? StatusBarStyle.LightContent : StatusBarStyle.DarkContent;
    public bool ShouldUseLightStyle
    {
        get => _shouldUseLightStyle;
        set
        {
            _shouldUseLightStyle = value;
            OnPropertyChanged(nameof(StatusBarStyle));
        }
    }
    public bool ShouldUseEdgeToEdge
    {
        get => _shouldUseEdgeToEdge;
        set
        {
            _shouldUseEdgeToEdge = value;
            OnPropertyChanged(nameof(ShouldUseEdgeToEdge));
            OnPropertyChanged(nameof(SimulatedDeviceInsets));
        }
    }

    public Thickness SimulatedDeviceInsets => _shouldUseEdgeToEdge ? new Thickness(0) : Insets.Current.DeviceInsetsThickness;

    void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SalmonPage());
    }
}

