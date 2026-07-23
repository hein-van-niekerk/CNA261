namespace DemoApi.Models;

public class WeatherRecord
{
    public int Id { get; set; }
    public string Summary { get; set; } = string.Empty;
    public int TemperatureC { get; set; }
}