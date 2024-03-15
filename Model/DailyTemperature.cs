using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp.Extended.UI.Controls;

namespace WeatherApplication.Model;

public class DailyTemperature
{
    public DateTime DateTime { get; set; }
    public string IconPhrase { get; set; }
    public bool IsDaylight { get; set; }
    public Temperature Temperature { get; set; }
    public int PrecipitationProbability { get; set; }
    public string WeatherTime => DateTime.ToString("HH:mm");
    public SKFileLottieImageSource SKFileLottieImageSource { get; set; }
}