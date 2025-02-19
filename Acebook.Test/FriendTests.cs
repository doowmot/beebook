namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class FriendTests
{
    [Test]
    public void TestFriendRequestIsSavedToDatabase()
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

        // SETUP FRIEND REQUEST
        Friend friendRequest = new Friend
        {
            UserId = user.Id,          // The current user's id (sender)
            FriendId = friendUser.Id,  // The friend's id (receiver)
            Status = FriendStatus.Pending
        };

        // TEST ACTIONS
        dbContext.Friends.Add(friendRequest);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Friend savedRequest = dbContext.Friends
            .FirstOrDefault(fr => fr.UserId == user.Id && fr.FriendId == friendUser.Id);

        Assert.That(savedRequest, Is.Not.Null, 
            "Friend request should be found in the database");
        Assert.That(savedRequest.Status, Is.EqualTo(FriendStatus.Pending), 
            "Friend request status should be Pending");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestFriendRequestCanBeRemoved()
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

        // SETUP FRIEND REQUEST
        Friend friendRequest = new Friend
        {
            UserId = user.Id,
            FriendId = friendUser.Id,
            Status = FriendStatus.Pending
        };
        dbContext.Friends.Add(friendRequest);
        dbContext.SaveChanges();

        // TEST ACTIONS
        dbContext.Friends.Remove(friendRequest);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Friend removedRequest = dbContext.Friends
            .FirstOrDefault(fr => fr.UserId == user.Id && fr.FriendId == friendUser.Id);
        
        Assert.That(removedRequest, Is.Null, 
            "Friend request should no longer exist in the database");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestFriendRequestCanBeAccepted()
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

        // SETUP FRIEND REQUEST
        Friend friendRequest = new Friend
        {
            UserId = user.Id,
            FriendId = friendUser.Id,
            Status = FriendStatus.Pending
        };
        dbContext.Friends.Add(friendRequest);
        dbContext.SaveChanges();

        // TEST ACTIONS
        friendRequest.Status = FriendStatus.Friends;
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Friend acceptedRequest = dbContext.Friends
            .FirstOrDefault(fr => fr.UserId == user.Id && fr.FriendId == friendUser.Id);

        Assert.That(acceptedRequest.Status, Is.EqualTo(FriendStatus.Friends),
            "Friend request status should be changed to Friends when accepted");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestFriendRequestCanBeDeclined()
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

        // SETUP FRIEND REQUEST
        Friend friendRequest = new Friend
        {
            UserId = user.Id,
            FriendId = friendUser.Id,
            Status = FriendStatus.Pending
        };
        dbContext.Friends.Add(friendRequest);
        dbContext.SaveChanges();

        // TEST ACTIONS
        friendRequest.Status = FriendStatus.Declined;
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Friend declinedRequest = dbContext.Friends
            .FirstOrDefault(fr => fr.UserId == user.Id && fr.FriendId == friendUser.Id);

        Assert.That(declinedRequest.Status, Is.EqualTo(FriendStatus.Declined),
            "Friend request status should be changed to Declined when rejected");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

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
            IsRead = false,
            DateCreated = DateTime.UtcNow  // Changed to UTC time
        };
        dbContext.Notifications.Add(notification);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Notification savedNotification = dbContext.Notifications
            .FirstOrDefault(n => n.UserId == friendUser.Id && n.SenderId == user.Id);

        Assert.That(savedNotification, Is.Not.Null,
            "Notification should be created when friend request is sent");
        Assert.That(savedNotification.IsRead, Is.False,
            "New notification should be unread");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestFriendRequestNavigationPropertiesAreCorrect()
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

        // SETUP FRIEND REQUEST
        Friend friendRequest = new Friend
        {
            UserId = user.Id,
            FriendId = friendUser.Id,
            Status = FriendStatus.Pending
        };
        dbContext.Friends.Add(friendRequest);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Friend savedRequest = dbContext.Friends
            .Include(f => f.User)
            .Include(f => f.FriendUser)
            .FirstOrDefault(fr => fr.UserId == user.Id && fr.FriendId == friendUser.Id);

        Assert.That(savedRequest.User.Email, Is.EqualTo("test1@example.com"),
            "User navigation property should point to correct sender");
        Assert.That(savedRequest.FriendUser.Email, Is.EqualTo("test2@example.com"),
            "FriendUser navigation property should point to correct receiver");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}


   // [Test]
    // public void TestCannotSendFriendRequestToSelf()
    // {
    //     // SETUP DATABASE
    //     Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
    //     AcebookDbContext dbContext = new AcebookDbContext();
    //     dbContext.Database.EnsureDeleted();
    //     dbContext.Database.EnsureCreated();

    //     // SETUP USER
    //     User user = new User("Test User 1", "test1@example.com", "password123");
    //     dbContext.Users.Add(user);
    //     dbContext.SaveChanges();

    //     // VERIFY RESULTS - Check that we can't add a self-referencing request
    //     int initialCount = dbContext.Friends.Count();
        
    //     Friend selfFriendRequest = new Friend
    //     {
    //         UserId = user.Id,
    //         FriendId = user.Id,
    //         Status = FriendStatus.Pending
    //     };
    //     dbContext.Friends.Add(selfFriendRequest);
    //     dbContext.SaveChanges();

    //     int finalCount = dbContext.Friends.Count();
    //     Assert.That(finalCount, Is.EqualTo(initialCount),
    //         "Should not allow friend request to self");

    //     // CLEANUP
    //     dbContext.Database.EnsureDeleted();
    //     dbContext.Dispose();
    // }

    // [Test]
    // public void TestDuplicateFriendRequestsAreNotAllowed()
    // {
    //     // SETUP DATABASE
    //     Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
    //     AcebookDbContext dbContext = new AcebookDbContext();
    //     dbContext.Database.EnsureDeleted();
    //     dbContext.Database.EnsureCreated();

    //     // SETUP USERS
    //     User user = new User("Test User 1", "test1@example.com", "password123");
    //     User friendUser = new User("Test User 2", "test2@example.com", "password456");
    //     dbContext.Users.Add(user);
    //     dbContext.Users.Add(friendUser);
    //     dbContext.SaveChanges();

    //     // SETUP FIRST FRIEND REQUEST
    //     Friend firstFriendRequest = new Friend
    //     {
    //         UserId = user.Id,
    //         FriendId = friendUser.Id,
    //         Status = FriendStatus.Pending
    //     };
    //     dbContext.Friends.Add(firstFriendRequest);
    //     dbContext.SaveChanges();

    //     // VERIFY RESULTS - Check that we can't add a duplicate request
    //     int initialCount = dbContext.Friends.Count();
        
    //     Friend duplicateFriendRequest = new Friend
    //     {
    //         UserId = user.Id,
    //         FriendId = friendUser.Id,
    //         Status = FriendStatus.Pending
    //     };
    //     dbContext.Friends.Add(duplicateFriendRequest);
    //     dbContext.SaveChanges();

    //     int finalCount = dbContext.Friends.Count();
    //     Assert.That(finalCount, Is.EqualTo(initialCount), 
    //         "Should not allow duplicate friend requests between the same users");

    //     // CLEANUP
    //     dbContext.Database.EnsureDeleted();
    //     dbContext.Dispose();
    // }