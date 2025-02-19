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
        Console.WriteLine($"Session User ID: {loggedInUserId}");

        if (!loggedInUserId.HasValue)
        {
            return RedirectToAction("New", "Sessions"); // Redirect to login if not logged in
        }

        Console.WriteLine($"Fetching notifications for User ID: {loggedInUserId.Value}");
        var notifications = dbContext.Notifications
            .Include(n => n.Sender) // Eager load the Sender relationship
            .Where(n => n.UserId == loggedInUserId.Value) // Fetch notifications for the logged-in user
            .OrderByDescending(n => n.DateCreated) // Order notifications by most recent
            .ToList();

        Console.WriteLine($"Found {notifications.Count} notifications for User ID: {loggedInUserId.Value}"); // Prints number of notifications received
        // Pass the notifications to the view
        return View(notifications);
    }
    public IActionResult MarkAsRead(int notificationId)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        var notification = dbContext.Notifications.Find(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            dbContext.SaveChanges();
        }
    
    return RedirectToAction("Index");
}

}
