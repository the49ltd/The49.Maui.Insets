namespace The49.Maui.Insets.Sample;

public partial class SalmonPage : ContentPage
{
	public SalmonPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }
}