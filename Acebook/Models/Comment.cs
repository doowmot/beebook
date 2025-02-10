namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class Comment
{
  [Key]
  public int Id {get; set;}
  public string? Comments {get; set;}
  public ICollection<Post>? Posts {get; set;}
  public User? User {get; set;}
}