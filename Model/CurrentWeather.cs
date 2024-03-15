using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Model;

public class CurrentWeather
{
    public string WeatherText { get; set; }
    public bool IsDayTime { get; set; }
    public Temperature Temperature { get; set; }
}

