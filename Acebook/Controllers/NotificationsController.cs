using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

public class NotificationsController : Controller
{
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(ILogger<NotificationsController> logger)
    {
        _logger = logger;
    }

    // Action to display notifications for the logged-in user
    [Route("notifications/index")]
    public IActionResult Index()
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");
        // Console.WriteLine($"Session User ID: {loggedInUserId}");

        if (!loggedInUserId.HasValue)
        {
            return RedirectToAction("New", "Sessions"); // Redirect to login if not logged in
        }

        var notifications = dbContext.Notifications
            .Include(n => n.Sender) // Eager load the Sender relationship
            .Where(n => n.UserId == loggedInUserId.Value) // Fetch notifications for the logged-in user
            .OrderByDescending(n => n.DateCreated) // Order notifications by most recent
            .ToList();

        // Console.WriteLine($"Found {notifications.Count} notifications for User ID: {loggedInUserId.Value}"); // Prints number of notifications received
        // Pass the notifications to the view
        return View(notifications);
    }
    [Route("notifications/acceptfriendrequest")]
    [HttpPost]
    public IActionResult AcceptFriendRequest(int notificationId)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");

        if (!loggedInUserId.HasValue)
        {
            return Unauthorized("User is not logged in.");
        }
        // Find the notification
        var notification = dbContext.Notifications
            .Include(n => n.Sender) // Load sender information
            .Where(n => n.Id == notificationId)
            .OrderByDescending(n => n.DateCreated)
            .FirstOrDefault();

        if (notification == null)
        {
            Console.WriteLine("Notification not found");
            return NotFound("Notification not found.");
        } 
        // Find the corresponding friend request
        var friendRequest = dbContext.Friends
            .Where(fr => fr.UserId == notification.SenderId && fr.FriendId == notification.UserId)
            .FirstOrDefault();
        
        if (friendRequest == null)
        {
            Console.WriteLine("Friend request not found");
            return NotFound("Friend request not found.");
        }
        friendRequest.Status = FriendStatus.Friends; // Accepts friend request
        dbContext.Notifications.Remove(notification); // Removes notification
        dbContext.SaveChanges(); // Save changes to database
        return RedirectToAction("Index", "Notifications");
    }

    [Route("notifications/declinefriendrequest")]
    [HttpPost]
    public IActionResult DeclineFriendRequest(int notificationId)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");

        // Find the notification
        var notification = dbContext.Notifications
            .Include(n => n.Sender) // Load sender information
            .Where(n => n.Id == notificationId)
            .OrderByDescending(n => n.DateCreated)
            .FirstOrDefault();
        if (notification == null)
        {
            return NotFound("Notification not found.");
        } 
        // Find and remove the corresponding friend request
        var friendRequest = dbContext.Friends
            .Where(fr => fr.UserId == notification.SenderId && fr.FriendId == notification.UserId)
            .FirstOrDefault();
        
        if (friendRequest != null)
        {
            dbContext.Friends.Remove(friendRequest); // Removes request from db
        }
        // friendRequest.Status = FriendStatus.Declined; // Accepts friend request
        dbContext.Notifications.Remove(notification); // Removes notification
        dbContext.SaveChanges(); // Save changes to database
        return RedirectToAction("Index", "Notifications");
    }
}
    