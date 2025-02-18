namespace acebook.Models;
using System.ComponentModel.DataAnnotations; // Needed for [Key] attribute
using System.ComponentModel.DataAnnotations.Schema; // Needed for [ForeignKey attribute]

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
}
