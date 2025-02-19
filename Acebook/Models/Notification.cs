namespace acebook.Models;
using System.ComponentModel.DataAnnotations; // Needed for [Key] attribute
using System.ComponentModel.DataAnnotations.Schema; // Needed for [ForeignKey attribute]

public class Notification
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("User")] // UserId is related to FriendId in the Friend model
    public int UserId { get; set; }  // The user who will receive the notification
    [ForeignKey("Sender")] // SenderId is related to UserId in the Friend model
    public int SenderId { get; set; } // The user who sent the friend request
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; }  // Receiver of the notification
    public virtual User Sender { get; set; }  // Sender of the friend request
}
