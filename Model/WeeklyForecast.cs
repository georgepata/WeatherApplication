using System;
using SkiaSharp.Extended.UI.Controls;

namespace WeatherApplication.Model;

public class WeeklyForecast
{
    public DateTime Date { get; set; }
    public Day Day { get; set; }
    public Temperature Temperature { get; set; }
    public SKLottieImageSource WeeklyWeatherAnimation { get; set; }
}

