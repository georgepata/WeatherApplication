using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApplication.Service;
using WeatherApplication.View;

namespace WeatherApplication.ViewModel;

public partial class SearchViewModel : BaseViewModel
{
	public SearchViewModel(WeatherService weatherService, IConnectivity connectivity, IGeolocation geolocation):
		base(weatherService, connectivity, geolocation)
	{

	}

    [ObservableProperty]
	private string inputText;
	private string verificare;

	[RelayCommand]
	public async Task DoSomething()
	{
		if (inputText != null)
		{
			verificare = InputText;
			InputText = String.Empty;
			await Shell.Current.GoToAsync("///" + $"{nameof(ChoosenCity)}", true);
		}
		else await Shell.Current.DisplayAlert("Atentie", "null", "Ok");
    }
}

