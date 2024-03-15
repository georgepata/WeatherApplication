using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Networking;
using WeatherApplication.Service;
using WeatherApplication.ViewModel;

namespace WeatherApplication;

public partial class MainPage : ContentPage
{

    public MainPage(YourCurrentLocationForecast yourCurrentLocationForecast)
    {
        InitializeComponent();
        BindingContext = yourCurrentLocationForecast;
    }
}


