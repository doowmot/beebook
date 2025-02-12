namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
  [Key]
  public int Id {get; set;}
  public string? Name {get; set;}
  public string? Email {get; set;}
  public string? Password {get; set;}
  public ICollection<Post>? Posts {get; set;}

  public User(string Name, string Email, string Password) 
  {
    this.Name = Name;
    this.Email = Email;
    this.Password = Password;
  }

  public void AddFriend()
  {

  }


}