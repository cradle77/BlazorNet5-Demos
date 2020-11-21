using BlazorNet5.Shared;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNet5.Server
{
    public class ServerWeatherService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private IMemoryCache _cache;

        public ServerWeatherService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<WeatherForecast[]> GetWeatherForecastsAsync()
        {
            return _cache.GetOrCreate("forecast", (c) =>
            {
                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            });

        }
    }
}
