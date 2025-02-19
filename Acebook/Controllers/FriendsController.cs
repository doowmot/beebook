using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

public class FriendsController : Controller
{
    private readonly ILogger<FriendsController> _logger;

    public FriendsController(ILogger<FriendsController> logger)
    {
        _logger = logger;
    }

    [Route("friends/sendfriendrequest")]
    [HttpPost]
    public IActionResult SendFriendRequest(int friendId)
    { 
        Console.WriteLine("SendFriendRequest method triggered");
        Console.WriteLine("Friend ID: " + friendId);
        AcebookDbContext dbContext = new AcebookDbContext();
        int? currentUserIdNullable = HttpContext.Session.GetInt32("user_id"); // Gets the current user's id
        if (currentUserIdNullable == null)
        {
            return BadRequest("User is not logged in.");
        }
        int currentUserId = currentUserIdNullable.Value;
        Friend friendRequest = new Friend // Creates a new friend request
        {
            UserId = currentUserId, // Sets the sender id to the current user's id
            FriendId = friendId, // Sets the receiver id to the friend's id
            Status = FriendStatus.Pending // Sets the request to pending 
        };
        dbContext.Friends.Add(friendRequest); // Adds new friend request to the database
        dbContext.SaveChanges(); // Saves friend request to database
        Console.WriteLine("Friend request sent successfully.");
        // Create a notification for the friend request
        Notification notification = new Notification
        {
            UserId = friendId,    // The user receiving the notification (FriendId)
            SenderId = currentUserId, // The user who sent the friend request (UserId)
            IsRead = false,       // Notification is unread by default
            DateCreated = DateTime.UtcNow // Set the current date/time for the notification
        };
        dbContext.Notifications.Add(notification); // Add the notification to the database
        dbContext.SaveChanges(); // Save the notification to the database

        Console.WriteLine("Notification for friend request sent.");

        return new RedirectResult("/profile/" + friendId); // After friend request sent, return/stay on profile of user friend request was sent to
    }
    [Route("friends/removefriendrequest")]
    [HttpPost]
    public IActionResult RemoveSentFriendRequest(int friendId)
    {
        Console.WriteLine("RemoveSentFriendRequest method triggered");
        AcebookDbContext dbContext = new AcebookDbContext();
        int currentUserId = HttpContext.Session.GetInt32("user_id").Value; // Gets the current user's id
        Friend friendRequest = null;
        if (dbContext.Friends != null)
        {
            Console.WriteLine($"CurrentUserId: {currentUserId}, FriendId: {friendId}");
            friendRequest = dbContext.Friends // Finds the friend request in the database
                .Where(fr => fr.UserId == currentUserId && fr.FriendId == friendId) // Where the sender id is the current user's id and the receiver id is the friend's id
                .FirstOrDefault();
                Console.WriteLine($"Friend request found: {friendRequest != null}");
        }
        if (friendRequest != null) // If friend request exists
        {
            dbContext.Friends.Remove(friendRequest); // Remove friend request from database
            dbContext.SaveChanges(); // Save changes
            Console.WriteLine("Friend request removed successfully.");
        }
        var updatedUserProfile = dbContext.Users
            .Where(u => u.Id == friendId)
            .FirstOrDefault();
        return new RedirectResult("/profile/" + friendId); // After friend request removed, return/stay on profile of user friend request was removed from
    }
}

