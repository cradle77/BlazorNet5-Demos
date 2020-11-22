using BlazorNet5.Shared;
using BlazorNet5.Shared.PageControllers;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorNet5.Client.PageControllers
{
    internal class WasmPlayersListPageController : IPlayersListPageController
    {
        private NotificationsManager _notifications;
        private HttpClient _http;

        public WasmPlayersListPageController(NotificationsManager notifications, HttpClient http)
        {
            _notifications = notifications;
            _http = http;

            _notifications.PlayerUpdatedAsync += this.LoadDataAsync;
        }

        public Virtualize<Player> Virtualize { get; set; }

        public event Action DataHasChanged;

        private async Task LoadDataAsync()
        {
            await this.Virtualize.RefreshDataAsync();

            this.DataHasChanged?.Invoke();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var response = await _http.DeleteAsync($"/api/players/{id}");
            response.EnsureSuccessStatusCode();

            await this.LoadDataAsync();
        }

        public void Dispose()
        {
            _notifications.PlayerUpdatedAsync -= this.LoadDataAsync;
        }

        public async ValueTask<ItemsProviderResult<Player>> GetPlayers(ItemsProviderRequest request)
        {
            var response = await _http.GetFromJsonAsync<PlayersResponse>($"/api/players?start={request.StartIndex}&count={request.Count}");

            return new ItemsProviderResult<Player>(response.Players, response.TotalCount);
        }
    }
}
