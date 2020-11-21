using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BlazorNet5.Client
{
    public class NotificationsManager : IAsyncDisposable
    {
        private NavigationManager _navigation;
        private HubConnection _hubConnection;

        public event Func<Task> PlayerUpdatedAsync;

        public NotificationsManager(NavigationManager navigation)
        {
            _navigation = navigation;
        }

        public async Task InitializeAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{_navigation.BaseUri}hubs/notifications")
                .Build();

            _hubConnection.On("playerUpdated", async () =>
            {
                await this.PlayerUpdatedAsync?.Invoke();
            });

            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
