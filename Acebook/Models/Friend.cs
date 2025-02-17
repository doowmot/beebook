namespace acebook.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Friend
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }  // The user sending/requesting the friendship
    public virtual User User { get; set; }  // Navigation property for sender

    [ForeignKey("FriendUser")]
    public int FriendId { get; set; }  // The user receiving the request
    public virtual User FriendUser { get; set; }  // Navigation property for receiver
    public FriendStatus Status { get; set; } = FriendStatus.Pending; // Default is 'pending' when a friend request is sent
}
public enum FriendStatus
{
    Null, // No friend request sent or received
    Pending,   // Friend request sent
    Friends,   // Accepted
    Declined   // Rejected
}