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
  public string? Profile_picture {get; set;}
  public ICollection<Post>? Posts {get; set;}

  
}