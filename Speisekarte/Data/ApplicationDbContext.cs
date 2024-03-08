using Microsoft.EntityFrameworkCore;

namespace Speisekarte.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Speise> Speisen { get; set; }
        public DbSet<Zutat> Zutaten { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}
