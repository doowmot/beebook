using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

public class NotificationsController : Controller
{
    private readonly AcebookDbContext _dbContext;

    public NotificationsController(AcebookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Action to display notifications for the logged-in user
    public IActionResult Index()
    {
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");

        if (!loggedInUserId.HasValue)
        {
            return RedirectToAction("New", "Sessions"); // Redirect to login if not logged in
        }

        var notifications = _dbContext.Notifications
            .Where(n => n.UserId == loggedInUserId.Value) // Fetch notifications for the logged-in user
            .OrderByDescending(n => n.DateCreated) // Order notifications by most recent
            .ToList();

        // Pass the notifications to the view
        return View(notifications);
    }
    public IActionResult MarkAsRead(int notificationId)
    {
        var notification = _dbContext.Notifications.Find(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            _dbContext.SaveChanges();
        }
    
    return RedirectToAction("Index");
}

}
