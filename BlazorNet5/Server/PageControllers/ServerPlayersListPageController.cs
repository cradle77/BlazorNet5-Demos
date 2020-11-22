using BlazorNet5.Server.Data;
using BlazorNet5.Shared;
using BlazorNet5.Shared.PageControllers;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNet5.Server.PageControllers
{
    internal class ServerPlayersListPageController : IPlayersListPageController
    {
        private MyContext _context;

        public ServerPlayersListPageController(MyContext context)
        {
            _context = context;
        }

        public Virtualize<Player> Virtualize { get; set; }

        public event Action DataHasChanged;

        public Task DeletePlayerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async ValueTask<ItemsProviderResult<Player>> GetPlayers(ItemsProviderRequest request)
        {
            IQueryable<Player> result = _context.Players
                .ApplySophisticatedAiAlgorithm()
                .OrderByDescending(x => x.ExtendNow)
                .ThenBy(x => x.ExpiryDate)
                .ThenBy(x => x.Id);

            result = result.Skip(request.StartIndex)
                .Take(request.Count);

            return new ItemsProviderResult<Player>(
                await result.ToListAsync(), 
                await _context.Players.CountAsync());
        }
    }
}
