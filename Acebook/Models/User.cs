namespace acebook.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

public class User
{
  [Key]
  public int Id {get; set;}
  public string? Name {get; set;}
  public string? Email {get; set;}
  public string? Password {get; set;}
  // public string? Profile_picture {get; set;}
  public ICollection<Post>? Posts {get; set;}
  // Navigation property for friends where the user is the sender
  public ICollection<Friend> FriendsSent { get; set; } = new List<Friend>();

  // Navigation property for friends where the user is the receiver
  public ICollection<Friend> FriendsReceived { get; set; } = new List<Friend>();

  public User(string name, string email, string password)
  {
      this.Name = name;
      this.Email = email;
      this.Password = password;
      this.Posts = new List<Post>();
  }

}