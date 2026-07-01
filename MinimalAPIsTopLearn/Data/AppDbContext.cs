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
        public DbSet<InstructorInfo> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CategoryInfo>().Property(p => p.Name).HasMaxLength(150);

            modelBuilder.Entity<InstructorInfo>().Property(p => p.Name).HasMaxLength(150);
            modelBuilder.Entity<InstructorInfo>().Property(p => p.Picture).IsUnicode();
        }
    }
}
