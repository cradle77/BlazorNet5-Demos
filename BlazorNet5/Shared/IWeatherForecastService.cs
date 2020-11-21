using System.Threading.Tasks;

namespace BlazorNet5.Shared
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetWeatherForecastsAsync();
    }
}
