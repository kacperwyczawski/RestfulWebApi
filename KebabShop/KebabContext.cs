using Microsoft.EntityFrameworkCore;

namespace KebabShop;

public class KebabContext : DbContext
{
    public KebabContext(DbContextOptions<KebabContext> options)
        : base(options)
    {
    }

    public DbSet<Kebab> Kebabs => Set<Kebab>();
}