using WeatherApplication.ViewModel;

namespace WeatherApplication.View;

public partial class ChoosenCity : ContentPage
{
	public ChoosenCity(YourCurrentLocationForecast yourCurrentLocationForecast)
	{
		InitializeComponent();
		BindingContext = yourCurrentLocationForecast;
	}
}
