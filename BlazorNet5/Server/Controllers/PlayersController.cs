using BlazorNet5.Server.Data;
using BlazorNet5.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BlazorNet5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public PlayersController(MyContext context, IHubContext<NotificationsHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<PlayersResponse>> GetPlayers(int? start = null, int? count = null)
        {
            IQueryable<Player> result = _context.Players
                .ApplySophisticatedAiAlgorithm()
                .OrderByDescending(x => x.ExtendNow)
                .ThenBy(x => x.ExpiryDate)
                .ThenBy(x => x.Id);

            result = result.Skip(start.GetValueOrDefault())
                .Take(count.GetValueOrDefault());

            var response = new PlayersResponse()
            {
                TotalCount = await _context.Players.CountAsync(),
                Players = await result.ToListAsync()
            };

            return response;
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, [FromBody] Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            // check number doesn't exist
            var clashingPlayer = await _context.Players
                .Where(x => x.Number == player.Number && x.Id != id).SingleOrDefaultAsync();

            if (clashingPlayer != null)
            {
                ModelState.AddModelError(nameof(player.Number),
                        $"The number '{player.Number}' is already in use by {clashingPlayer.Name}");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _hubContext.Clients.All.SendAsync("playerUpdated");

            return NoContent();
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer([FromBody] Player player)
        {
            // check number doesn't exist
            var clashingPlayer = await _context.Players.Where(x => x.Number == player.Number).SingleOrDefaultAsync();

            if (clashingPlayer != null)
            {
                ModelState.AddModelError(nameof(player.Number),
                        $"The number '{player.Number}' is already in use by {clashingPlayer.Name}");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("playerUpdated");

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("playerUpdated");

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
