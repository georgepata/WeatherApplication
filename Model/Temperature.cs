using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApplication.Model;

public class Temperature
{
    public Minimum Minimum { get; set; }
    public Maximum Maximum { get; set; }
    public Metric Metric {get; set;}
    public Imperial Imperial {get; set;}
    public double Value { get; set; }
    public string Unit { get; set; }
    public int CelsiusValue => (int)((double)5/9 * (Value - 32));
}
public class Minimum
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public int CelsiusValue => (int)((double)5/9 * (Value - 32));
}
public class Maximum
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public int CelsiusValue => (int)((double)5 / 9 * (Value - 32));
}

public class Metric
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public int UnitType { get; set; }
}

public class Imperial
{
    public double Value { get; set; }
    public string Unit { get; set; }
    public int UnitType { get; set; }
}