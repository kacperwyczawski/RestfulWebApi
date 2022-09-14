using Microsoft.EntityFrameworkCore;

namespace KebabShop;

public class KebabDatabaseContext : DbContext
{
    public KebabDatabaseContext(DbContextOptions<KebabDatabaseContext> options) : base(options) { }
    
    public DbSet<Kebab> Kebabs { get; set; }
}