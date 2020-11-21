using BlazorNet5.Shared;
using System.Linq;

namespace BlazorNet5.Server.Data
{
    public static class PlayerExtensions
    {
        public static IQueryable<Player> ApplySophisticatedAiAlgorithm(this IQueryable<Player> players)
        {
            return players.Select(player => new Player()
            {
                Id = player.Id,
                ExpiryDate = player.ExpiryDate,
                Name = player.Name,
                Number = player.Number,
                Photo = player.Photo,
                Role = player.Role,
                ExtendNow = player.Name == "Gianluigi Donnarumma" || player.Name == "Zlatan Ibrahimovic"
            });
        }
    }
}
