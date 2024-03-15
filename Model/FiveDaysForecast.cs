using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Model;

public class FiveDaysForecast
{
    public List<DailyForecast> DailyForecasts { get; set; }
}

public partial class DailyForecast
{
    public DateTime Date { get; set; }
    public Day Day { get; set; }
}

public class Day
{
    public string IconPhrase { get; set; }
}

