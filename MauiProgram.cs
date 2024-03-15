using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WeatherApplication.Service;
using WeatherApplication.View;
using WeatherApplication.ViewModel;

namespace WeatherApplication;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton<WeatherService>();
        builder.Services.AddSingleton<YourCurrentLocationForecast>();
		builder.Services.AddSingleton<SearchViewModel>();
        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<SearchView>();

        return builder.Build();
	}
}

