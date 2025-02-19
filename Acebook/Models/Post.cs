namespace acebook.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Post
{
  [Key]
  public int Id {get; set;}
  public string? Content {get; set;}
  [ForeignKey("User")]
  public int UserId {get; set;}
  public User? User {get; set;}
  public ICollection<Comment>? Comments {get; set;}

  public int LikesCount { get; set; }
  public ICollection<UserLike>? UserLikes {get; set;}

    private DateTime _createdAt = DateTime.UtcNow; // Always store in UTC

    [Column("DateTimeOfPost")] // Maps to the correct DB column
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value != default 
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc) 
            : DateTime.UtcNow; // Fallback to UtcNow if null/default
    }
}
