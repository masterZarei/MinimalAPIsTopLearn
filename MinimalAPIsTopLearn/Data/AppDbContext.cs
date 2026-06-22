using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsTopLearn.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
