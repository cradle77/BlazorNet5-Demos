using BlazorNet5.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorNet5.Server.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
