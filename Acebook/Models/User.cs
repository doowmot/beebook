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

  
  public ICollection<User>? Friends {get; set;}  // Created list to store all friends this user has added

  // Constructor so we can create new users
  public User(string? name, string? email, string? password) 
  {
    this.Name = name;
    this.Email = email;
    this.Password = password;
    this.Posts = new List<Post>();  
    this.Friends = new List<User>();
  }
  public User(){}

  // Method to test adding another user as a friend
  public string AddFriend(User friend)
  {
    if (Friends.Contains(friend))
    {
        return "Error: This person is already a friend";
    }
    Friends.Add(friend);
        return "Friend added successfully";
  }    

  // Method to test removing another user as a friend
  public string RemoveFriend(User friend)
  {
      if (!Friends.Contains(friend))
      {
          return "Error: Cannot remove this person as they are not a friend";
      }
      Friends.Remove(friend);
      return "Friend removed successfully";
  }

}