namespace acebook.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Win32.SafeHandles;

public class Settings
{
    [Key]
    public int Id { get; set; }
    public string NewName {get; set;}
    public string NewEmail { get; set; }    
    public string NewProfilePicture {get; set;}

    public Settings(string NewName, string NewEmail, string NewProfilePicture)
    {
        this.NewName = NewName;
        this.NewEmail = NewEmail;
        this.NewProfilePicture = NewProfilePicture;
    }
}