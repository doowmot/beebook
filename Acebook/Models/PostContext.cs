namespace acebook.Models;
using Microsoft.EntityFrameworkCore;

public class PostContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }

    public string? DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=acebook_csharp");
}
