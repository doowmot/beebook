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
        // Create a notification for the friend request
        Notification notification = new Notification
        {
            UserId = friendId,    // The user receiving the notification (FriendId)
            SenderId = currentUserId, // The user who sent the friend request (UserId)
            DateCreated = DateTime.UtcNow // Set the current date/time for the notification
        };
        dbContext.Notifications.Add(notification); // Add the notification to the database
        dbContext.SaveChanges(); // Save the notification to the database
        return RedirectToAction("Profile", "Users", new { id = friendId });
    }
    
    [HttpPost]
    [Route("Friends/RemoveFriend")]
    public IActionResult RemoveFriend(int friendId)
    {
        Console.WriteLine("RemoveFriend method triggered");
        AcebookDbContext dbContext = new AcebookDbContext();
        int currentUserId = HttpContext.Session.GetInt32("user_id").Value; // Get logged-in user ID

        // Find the friend relationship (for both possible directions)
        var friendship = dbContext.Friends
            .Where(f => (f.UserId == currentUserId && f.FriendId == friendId) ||
                        (f.UserId == friendId && f.FriendId == currentUserId))
            .FirstOrDefault();

        if (friendship != null) // If a friendship exists, remove it
        {
            dbContext.Friends.Remove(friendship);
            dbContext.SaveChanges();
            Console.WriteLine("Friend removed successfully.");
        }

        return new RedirectResult("/profile/" + friendId); // Stay on profile page
}
}

