namespace acebook.Models;
using System.ComponentModel.DataAnnotations;

public class Friend
{
    public int Id { get; set; }
    public int UserId { get; set; }  // The user sending/requesting the friendship
    public int FriendId { get; set; }  // The user receiving the request

    public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending; // Default is 'pending' when a friend request is sent

    public virtual User User { get; set; }  // Navigation property for sender
    public virtual User FriendUser { get; set; }  // Navigation property for receiver
}
