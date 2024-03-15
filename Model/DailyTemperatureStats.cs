using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Model;



public class DailyTemperatureStats
{
    public Headline Headline { get; set; }
    public List<DailyForecast> DailyForecasts { get; set; }
    public DailyForecast Data => DailyForecasts?.FirstOrDefault();
}

public class Headline
{
    public string Text { get; set; }
}

public partial class DailyForecast
{
    public Temperature Temperature { get; set; }
}


