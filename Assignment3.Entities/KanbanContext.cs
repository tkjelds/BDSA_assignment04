using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public KanbanContext(DbContextOptions<KanbanContext> options) 
    : base(options){
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
      
            modelBuilder.Entity<Task>().HasOne(t => t.AssignedTo).WithMany(u => u.Tasks);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>().HasMany(u => u.Tasks).WithOne(t => t.AssignedTo);

            modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();
    }
}

