using System;
using Newtonsoft.Json;

namespace WeatherApplication.Model;
public class CustomLocation
{
    public string Key { get; set; }
    public string LocalizedName { get; set; }
    public Region Region { get; set; }
    public Country Country { get; set; }
    public override string ToString()
    {
        return Country.LocalizedName + ", " + LocalizedName;
    }
}

public class Country
{
    public string ID { get; set; }
    public string LocalizedName { get; set; }
}

public class Region
{
    public string ID { get; set; }
    public string LocalizedName { get; set; }
}
