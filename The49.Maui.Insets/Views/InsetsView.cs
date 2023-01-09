namespace The49.Maui.Insets;

public class InsetsView: ContentView
{
    public InsetsView(): base()
    {
        SetBinding(PaddingProperty, new Binding(nameof(Insets.InsetsThickness), source: Insets.Current));
    }
}

public class TopInsetView : ContentView
{
    public TopInsetView() : base()
    {
        SetBinding(PaddingProperty, new Binding(nameof(Insets.TopInsetThickness), source: Insets.Current));
    }
}

public class BottomInsetView : ContentView
{
    public BottomInsetView() : base()
    {
        SetBinding(PaddingProperty, new Binding(nameof(Insets.BottomInsetThickness), source: Insets.Current));
    }
}
