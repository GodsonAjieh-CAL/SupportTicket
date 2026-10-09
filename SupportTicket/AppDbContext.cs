using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SupportTicket
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<SupportTicket> Tickets { get; set; }
    }
}
