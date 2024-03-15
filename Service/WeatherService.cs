using System;
using System.Net.Http.Json;
using WeatherApplication.Model;
using Newtonsoft.Json;
using WeatherApplication.Model;

namespace WeatherApplication.Service;

public class WeatherService
{
    private HttpClient httpClient; 
    private CustomLocation location;
    private CurrentWeather currentWeather;
    private DailyTemperatureStats dailyTemperatureStats;
    private List<DailyTemperature> listOfDailyTemperatures;
    private FiveDaysForecast fiveDaysForecast;
    public WeatherService()
    {
        httpClient = new HttpClient();
    }

    //returneaza cheia locatiei (orasului)
    public async Task<CustomLocation> GetLocationKeyServiceAsync(string? userCityLocation){
        string userLocation = userCityLocation;
        var url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=PkeeAAKiW2gn7wA5ky8BzBoPrcmRCkfj&q={userLocation}";
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var searchResults = await response.Content.ReadFromJsonAsync<List<CustomLocation>>();
            location =searchResults.ElementAt(0);
            Console.WriteLine("Textul este: " + location.Key);
        }
        return location;
    }

    //returneaza temp minima si maxima(ce ne intereseaza)
    public async Task<DailyTemperatureStats> GetDailyTemperatureStatsServiceAsync(string Key){
        dailyTemperatureStats= null;
        var url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{Key}?apikey=PkeeAAKiW2gn7wA5ky8BzBoPrcmRCkfj";
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode){
            try{
                var responseBody = await response.Content.ReadFromJsonAsync<DailyTemperatureStats>();
                dailyTemperatureStats = responseBody;
            } catch (Exception e){
                await Shell.Current.DisplayAlert("Error", e.Message, "Ok");
            }
        }
        return dailyTemperatureStats;
    }    

    //returneaza vremea actuala
    public async Task<CurrentWeather> GetCurrentWeatherServiceAsync(string Key){
        currentWeather=null;
        var url = $"http://dataservice.accuweather.com/currentconditions/v1/{Key}?apikey=PkeeAAKiW2gn7wA5ky8BzBoPrcmRCkfj";
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode){
            try{
                var responseBody = await response.Content.ReadFromJsonAsync<List<CurrentWeather>>();
                currentWeather = responseBody.ElementAt(0);
            } catch (Exception e){
                await Shell.Current.DisplayAlert("Error", e.Message, "Ok");
            }
        }
        return currentWeather;
    }


    //vremea pe parcursul urmatoarelor 12 ore.
    public async Task<List<DailyTemperature>> GetDailyTemperaturesServiceAsync(string Key){
        var url = $"http://dataservice.accuweather.com/forecasts/v1/hourly/12hour/{Key}?apikey=PkeeAAKiW2gn7wA5ky8BzBoPrcmRCkfj";
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode){
            try{
                var responseBody  = await response.Content.ReadFromJsonAsync<List<DailyTemperature>>();
                listOfDailyTemperatures = responseBody;
            } catch (Exception e){
                await Shell.Current.DisplayAlert("Error!", $"{e.Message}" , "Ok");
            }
        }
        return listOfDailyTemperatures;
    }

    //returneaza vremea pe urmatoarele cinci zile
    public async Task<FiveDaysForecast> GetFiveDaysForecastServiceAsync(string Key){
        var url = "http://dataservice.accuweather.com/forecasts/v1/daily/5day/1-287719_1_AL?apikey=PkeeAAKiW2gn7wA5ky8BzBoPrcmRCkfj&metric=true&details=true";
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode){
            try{
                var responseBody = await response.Content.ReadFromJsonAsync<FiveDaysForecast>();
                fiveDaysForecast = responseBody;
            } catch(Exception e){
                await Shell.Current.DisplayAlert("Error\n", e.Message, "Ok");
            }
        }
        return fiveDaysForecast;
    }
}

