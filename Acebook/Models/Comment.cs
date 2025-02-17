namespace acebook.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Comments")] // Added as it was appearing as 'comment' in Fliss database
  public class Comment
{
  [Key]
  public int Id {get; set;}
  public string? Comments {get; set;}
  
  public DateTime DateTime {get; set;}

 [Required]
  public int UserId { get; set; }
  public User? User {get; set;}
  // Foreign Key for Post
  [Required]
  public int PostId { get; set; }
  public Post? Post { get; set; } // Navigation property
}

 // public ICollection<Post>? Posts {get; set;}