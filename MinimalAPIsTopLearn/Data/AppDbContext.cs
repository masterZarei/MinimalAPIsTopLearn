using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CategoryInfo> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CategoryInfo>().Property(p => p.Name).HasMaxLength(150);
        }
    }
}
