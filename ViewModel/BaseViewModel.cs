using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp.Extended.UI.Controls;
using WeatherApplication.Model;
using WeatherApplication.Service;

namespace WeatherApplication.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    public required string title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(isNotBusy))]
    public bool isBusy;
    public bool isNotBusy => !isBusy;

    public WeatherService weatherService;
    public IConnectivity connectivity;
    public IGeolocation geolocation;
    [ObservableProperty]
    public CustomLocation userLocation;
    [ObservableProperty]
    public DailyTemperatureStats dailyTemperatureStats;
    [ObservableProperty]
    public CurrentWeather currentWeather;
    public ObservableCollection<DailyTemperature> ListOfDailyTemperatures { get; } = new();
    [ObservableProperty]
    public SKFileLottieImageSource currentWeatherAnimation = new SKFileLottieImageSource();
    [ObservableProperty]
    public SKFileLottieImageSource weatherIndicator = new SKFileLottieImageSource();
    [ObservableProperty]
    public FiveDaysForecast fiveDaysForecast;
    public ObservableCollection<WeeklyForecast> ListOfFiveDaysForecasts { get; } = new();
    [ObservableProperty]
    public bool isRefreshing;

    public BaseViewModel(WeatherService weatherService, IConnectivity connectivity, IGeolocation geolocation)
    {
        this.weatherService = weatherService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
        title = "Weather";
        WeatherIndicator.File = "weather_indicator.json";
    }

    protected async Task InitializeAsync(string inputCity){
        IsBusy=true;
        Permissions.LocationWhenInUse locationWhenInUse = new Permissions.LocationWhenInUse();
        PermissionStatus permissionStatus = await locationWhenInUse.RequestAsync();
        if (permissionStatus == PermissionStatus.Granted)
        {
            await GetLocationKeyAsync(inputCity);
            callUpMethodsAsync();
        }
        else
        {
            await Shell.Current.DisplayAlert("Information!", "User denied the policy", "Ok");
            IsBusy = false;
        }
    }

    [RelayCommand]
    protected async Task callUpMethodsAsync(){
        await GetDailyTemperatureStatsAsync();
        await GetCurrentWeatherAsync();
        await GetDailyTemperaturesAsync();
        await GetFiveDaysForecastAsync();
        IsBusy=false;
        IsRefreshing = false;
    }
    protected async Task GetDailyTemperaturesAsync(){
        var temperatures = await weatherService.GetDailyTemperaturesServiceAsync(UserLocation.Key);
        // StringBuilder sb = new StringBuilder();
        // foreach (var x in listOfDailyTemperatures)
        // {
        //    try{
        //        sb.Append(x.DateTime + ": "+x.Temperature.Value+"\n");
        //    } catch(Exception e){
        //        await Shell.Current.DisplayAlert("Error", e.Message, "Ok");
        //    }
        // }
        if (ListOfDailyTemperatures.Count()>0)
            ListOfDailyTemperatures.Clear();
        foreach (var x in temperatures){
            x.SKFileLottieImageSource = WeatherConverterToJson(x.IconPhrase, x.IsDaylight, nameof(x.SKFileLottieImageSource));
            ListOfDailyTemperatures.Add(x);
        }
        //await Shell.Current.DisplayAlert("Mesaj", sb.ToString(), "Ok");
    }

    // observableproperty pentru currentWeather;
    protected async Task GetCurrentWeatherAsync(){
        CurrentWeather = await weatherService.GetCurrentWeatherServiceAsync(UserLocation.Key);
        CurrentWeatherAnimation= WeatherConverterToJson(CurrentWeather.WeatherText, CurrentWeather.IsDayTime, nameof(CurrentWeatherAnimation));
        //await Shell.Current.DisplayAlert("MESAJ", $"{CurrentWeather.WeatherText}: {CurrentWeather.Temperature.Metric.Value}", "Ok");
    }

    // prognoza meteo pe toata ziua. observableproperty pentru dailyTemperatureStats
    protected async Task GetDailyTemperatureStatsAsync(){
        DailyTemperatureStats= await weatherService.GetDailyTemperatureStatsServiceAsync(UserLocation.Key);
        //dailyTemperatureStats.Data = dailyTemperatureStats.DailyForecasts.FirstOrDefault();
        StringBuilder sb =new StringBuilder();
        foreach (var x in dailyTemperatureStats.DailyForecasts){
            sb.Append(x.Temperature.Minimum.Unit + ": " + x.Temperature.Minimum.Value + "\n");
        }
        //await Shell.Current.DisplayAlert("Mesaj", sb.ToString(), "Ok");
    }
    /*
    am obtinut cheia orasului user-ului pentru viitoarele request-uri api. metoda se va schimba la un moment dat pt ca YourCurrentLocationForecast.GetLocationKeyAsync() seamana cu
    WeatherService.GetLocationKeyServiceAsync(). observableproperty pentru userLocation.
    */
    public async Task GetLocationKeyAsync(string inputCity)
    {
        string city = await GetUserLocationAsync();
        if (inputCity is not null)
            city = inputCity;
        try {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue!", "Check your internet and try again", "Ok");
            }
            UserLocation = await weatherService.GetLocationKeyServiceAsync(city);
        } catch (Exception e) {
            Debug.WriteLine(e);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get location {e.Message}", "Ok");
        }
    }

    //am obtinut numele orasului user-ului folosindu-ne de un third party website dupa ce user ul a fost de acord pentru partajarea locatiei
    public async Task<string> GetUserLocationAsync(){
        string? city=null;
        //Permissions.LocationWhenInUse locationWhenInUse = new Permissions.LocationWhenInUse();
        //PermissionStatus permissionStatus = await locationWhenInUse.RequestAsync();
        //if (permissionStatus == PermissionStatus.Granted) {
            //try
            //{
            //    permissionStatus = await locationWhenInUse.RequestAsync();
            //} catch (Exception ex)
            //{
            //    await Shell.Current.DisplayAlert("Error!", $"{ex.Message}", "OK");
            //}
            try
            {
                string externalIP = new WebClient().DownloadString("http://icanhazip.com").Trim();
                var locationData = new WebClient().DownloadString($"http://ip-api.com/json/{externalIP}");
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(locationData);
                city = result.city;
                Console.WriteLine($"Your city is: {city}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        //} else await Shell.Current.DisplayAlert("Information!", "User denied the policy", "Ok");
        return city;
    }


    protected async Task GetFiveDaysForecastAsync(){
        if (ListOfFiveDaysForecasts.Count!=0)
            ListOfFiveDaysForecasts.Clear();
        FiveDaysForecast = await weatherService.GetFiveDaysForecastServiceAsync(UserLocation.Key);
        foreach(var x in FiveDaysForecast.DailyForecasts)
        {
            Console.WriteLine(x.Day.IconPhrase);
            WeeklyForecast weeklyForecast = new WeeklyForecast()
            {
                Date = x.Date,
                Day = x.Day,
                Temperature = x.Temperature,
                WeeklyWeatherAnimation = WeatherConverterToJson(x.Day.IconPhrase, true, nameof(GetFiveDaysForecastAsync))
            };
            ListOfFiveDaysForecasts.Add(weeklyForecast);
        }
    }

    protected SKFileLottieImageSource WeatherConverterToJson(string IconPhrase, bool IsDaylight, string nameOfCaller)
    {
        SKFileLottieImageSource sKFileLottieImageSource = new SKFileLottieImageSource();
        switch (IconPhrase)
        {
            case "Cloudy":
            case "Mostly cloudy":
            case "Partly cloudy":
            case "Intermittent clouds":
                if (IsDaylight){
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "day_cloudy.json";
                    else if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                        CurrentWeatherAnimation.File = "day_cloudy.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "day_cloudy.json";
                }
                else {
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "night_cloudy.json";
                    else if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                        CurrentWeatherAnimation.File = "night_cloudy.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "night_cloudy.json";
                }
                break;
            case "Clear":
            case "Mostly clear":
                if (IsDaylight) {
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "day_clear.json";
                    else  if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                        CurrentWeatherAnimation.File = "day_clear.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "day_clear.json";
                }
                else{ 
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "night_clear.json";
                    else  if(nameOfCaller.Equals("CurrentWeatherAnimation")) 
                        CurrentWeatherAnimation.File = "night_clear.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "night_clear.json";
                }
                break;
            case "Mostly sunny":
            case "Partly sunny":
                if (IsDaylight)
                {
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "partly_sunny.json";
                    else  if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                        CurrentWeatherAnimation.File = "partly_sunny.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "partly_sunny.json";
                }
                else
                {
                    if (nameOfCaller.Equals("SKFileLottieImageSource"))
                        sKFileLottieImageSource.File = "night_clear.json"; // else ul acesta nu e corect
                    else  if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                        CurrentWeatherAnimation.File = "night_clear.json";
                    else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                        sKFileLottieImageSource.File = "night_clear.json";
                }
                break;
            case "Sunny":
                if (IsDaylight)
                    {
                        if (nameOfCaller.Equals("SKFileLottieImageSource"))    
                            sKFileLottieImageSource.File = "day_sunny.json";
                        else  if(nameOfCaller.Equals("CurrentWeatherAnimation"))
                            CurrentWeatherAnimation.File = "day_sunny.json";
                        else if (nameOfCaller.Equals("GetFiveDaysForecastAsync"))
                            sKFileLottieImageSource.File = "day_sunny.json";
                    }
                break;
        }
        if (nameOfCaller.Equals("SKFileLottieImageSource") || nameOfCaller.Equals("GetFiveDaysForecastAsync"))
            return sKFileLottieImageSource;
        return CurrentWeatherAnimation;
    }
}

