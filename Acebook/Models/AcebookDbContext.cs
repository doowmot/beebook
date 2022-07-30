namespace acebook.Models;
using Microsoft.EntityFrameworkCore;

public class AcebookDbContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }

    public string? DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=acebook_csharp");
}
