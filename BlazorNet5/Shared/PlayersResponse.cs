using System.Collections.Generic;

namespace BlazorNet5.Shared
{
    public class PlayersResponse
    {
        public int TotalCount { get; set; }
        public List<Player> Players { get; set; }
    }
}
