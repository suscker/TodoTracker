using Microsoft.EntityFrameworkCore;
using TodoTracker.Models;

namespace TodoTracker.Data;

public class AppDbContext : DbContext
{

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.OwnerId);
    }

}