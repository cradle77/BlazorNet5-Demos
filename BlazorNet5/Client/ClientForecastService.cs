using BlazorNet5.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorNet5.Client
{
    public class ClientForecastService : IWeatherForecastService
    {
        private HttpClient _client;

        public ClientForecastService(HttpClient client)
        {
            _client = client;
        }

        public Task<WeatherForecast[]> GetWeatherForecastsAsync()
        {
            return _client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
        }
    }
}
