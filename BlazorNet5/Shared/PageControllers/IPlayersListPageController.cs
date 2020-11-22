using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Threading.Tasks;

namespace BlazorNet5.Shared.PageControllers
{
    public interface IPlayersListPageController : IDisposable
    {
        Virtualize<Player> Virtualize { get; set; }

        ValueTask<ItemsProviderResult<Player>> GetPlayers(ItemsProviderRequest request);

        Task DeletePlayerAsync(int id);

        event Action DataHasChanged;
    }
}
