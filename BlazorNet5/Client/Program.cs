using BlazorNet5.Client.PageControllers;
using BlazorNet5.Shared;
using BlazorNet5.Shared.PageControllers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorNet5.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<NotificationsManager>();
            builder.Services.AddScoped<IWeatherForecastService, ClientForecastService>();
            builder.Services.AddTransient<IPlayersListPageController, WasmPlayersListPageController>();

            var host = builder.Build();

            var notificationsManager = host.Services.GetRequiredService<NotificationsManager>();
            await notificationsManager.InitializeAsync();

            await host.RunAsync();
        }
    }
}
