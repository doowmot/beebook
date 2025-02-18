using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

public class FriendsController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public FriendsController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult SendFriendRequest(int friendId)
    { 
        AcebookDbContext dbContext = new AcebookDbContext();
        int currentUserId = HttpContext.Session.GetInt32("user_id").Value; // Gets the current user's id
        Friend friendRequest = new Friend // Creates a new friend request
        {
            UserId = currentUserId, // Sets the sender id to the current user's id
            FriendId = friendId, // Sets the receiver id to the friend's id
            Status = FriendStatus.Pending // Sets the request to pending 
        };
        dbContext.Friends.Add(friendRequest); // Adds new friend request to the database
        dbContext.SaveChanges(); // Saves friend request to database
        return new RedirectResult("/profile/" + friendId); // After friend request sent, return/stay on profile of user friend request was sent to
    }
    [HttpPost]
    public IActionResult RemoveSentFriendRequest(int friendId)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        int currentUserId = HttpContext.Session.GetInt32("user_id").Value; // Gets the current user's id
        Friend friendRequest = dbContext.Friends // Finds the friend request in the database
            .Where(fr => fr.UserId == currentUserId && fr.FriendId == friendId) // Where the sender id is the current user's id and the receiver id is the friend's id
            .FirstOrDefault();
        if (friendRequest != null) // If friend request exists
        {
            dbContext.Friends.Remove(friendRequest); // Remove friend request from database
            dbContext.SaveChanges(); // Save changes
        }
        return new RedirectResult("/profile/" + friendId); // After friend request removed, return/stay on profile of user friend request was removed from
    }
}

