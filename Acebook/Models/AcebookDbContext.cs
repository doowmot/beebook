namespace acebook.Models;
using Microsoft.EntityFrameworkCore;

public class AcebookDbContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<UserLike>? UserLikes { get; set; }
    public DbSet<Friend>? Friends { get ; set; }


    public string? DbPath { get; }

    public string? GetDatabaseName() {
      string? DatabaseNameArg = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "acebook_csharp_development";

      if( DatabaseNameArg == null)
      {
        System.Console.WriteLine(
          "DATABASE_NAME is null. Defaulting to test database."
        );
        return "acebook_csharp_test";
      }
      else
      {
        System.Console.WriteLine(
          "Connecting to " + DatabaseNameArg
        );
        return DatabaseNameArg;
      }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=" + GetDatabaseName());
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    // Ensure Posts include Users automatically
        modelBuilder.Entity<Post>()
            .Navigation(post => post.User)
            .AutoInclude();

    // 🔥 Explicitly define the Friend relationships
        modelBuilder.Entity<Friend>()
            .HasOne(f => f.User)             // Friend request sender
            .WithMany(u => u.FriendsSent)    // One user can send multiple requests
            .HasForeignKey(f => f.UserId)    
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.FriendUser)       // Friend request receiver
            .WithMany(u => u.FriendsReceived) // One user can receive multiple requests
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);
}   
}
