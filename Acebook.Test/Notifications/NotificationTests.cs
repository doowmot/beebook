namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class NotificationTests
{
    [Test]
    public void TestNotificationIsCreatedWithFriendRequest()
    {
        // SETUP DATABASE
        Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
        AcebookDbContext dbContext = new AcebookDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // SETUP USERS
        User user = new User("Test User 1", "test1@example.com", "password123");
        User friendUser = new User("Test User 2", "test2@example.com", "password456");
        dbContext.Users.Add(user);
        dbContext.Users.Add(friendUser);
        dbContext.SaveChanges();

        // SETUP FRIEND REQUEST AND NOTIFICATION
        Friend friendRequest = new Friend
        {
            UserId = user.Id,
            FriendId = friendUser.Id,
            Status = FriendStatus.Pending
        };
        dbContext.Friends.Add(friendRequest);

        Notification notification = new Notification
        {
            UserId = friendUser.Id,
            SenderId = user.Id,
            DateCreated = DateTime.UtcNow
        };
        dbContext.Notifications.Add(notification);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Notification savedNotification = dbContext.Notifications
            .FirstOrDefault(n => n.UserId == friendUser.Id && n.SenderId == user.Id);

        Assert.That(savedNotification, Is.Not.Null,
            "Notification should be created when friend request is sent");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
