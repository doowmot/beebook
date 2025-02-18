namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class FriendTests
{
    private AcebookDbContext _dbContext;
    private User _user1;
    private User _user2;

    [SetUp]
    public void Setup()
    {
        // Set the environment variable for tests
        Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
        
        // Create fresh database for each test
        _dbContext = new AcebookDbContext();
        
        // Ensure database exists
        _dbContext.Database.EnsureDeleted(); // Start fresh
        _dbContext.Database.EnsureCreated(); // Create the database with all tables

        // Create two test users
        _user1 = new User("Test User 1", "test1@example.com", "password123");
        _user2 = new User("Test User 2", "test2@example.com", "password456");
        
        // Add users to database
        _dbContext.Users.Add(_user1);
        _dbContext.Users.Add(_user2);
        _dbContext.SaveChanges();
    }

    [Test]
    public void TestFriendRequestIsSavedToDatabase()
    {
        // Create a new friend request
        var friendRequest = new Friend
        {
            UserId = _user1.Id,        // From user1
            FriendId = _user2.Id,      // To user2
            Status = FriendStatus.Pending
        };

        // Save it to database
        _dbContext.Friends.Add(friendRequest);
        _dbContext.SaveChanges();

        // Check if it exists in database
        var savedRequest = _dbContext.Friends
            .FirstOrDefault(f => f.UserId == _user1.Id && f.FriendId == _user2.Id);

        Assert.That(savedRequest, Is.Not.Null);
        Assert.That(savedRequest.Status, Is.EqualTo(FriendStatus.Pending));
    }

    [Test]
    public void TestFriendRequestCanBeRemoved()
    {
        // First create and save a friend request
        var friendRequest = new Friend
        {
            UserId = _user1.Id,
            FriendId = _user2.Id,
            Status = FriendStatus.Pending
        };
        _dbContext.Friends.Add(friendRequest);
        _dbContext.SaveChanges();

        // Remove the friend request
        _dbContext.Friends.Remove(friendRequest);
        _dbContext.SaveChanges();

        // Check it's gone from database
        var removedRequest = _dbContext.Friends
            .FirstOrDefault(f => f.UserId == _user1.Id && f.FriendId == _user2.Id);
        
        Assert.That(removedRequest, Is.Null);
    }

    [Test]
    public void TestFriendRequestCanBeAccepted()
    {
        // Create a new friend request
        var friendRequest = new Friend
        {
            UserId = _user1.Id,
            FriendId = _user2.Id,
            Status = FriendStatus.Pending
        };

        // Save it to database
        _dbContext.Friends.Add(friendRequest);
        _dbContext.SaveChanges();

        // Update status to Friends
        friendRequest.Status = FriendStatus.Friends;
        _dbContext.SaveChanges();

        // Check if status was updated
        var acceptedRequest = _dbContext.Friends
            .FirstOrDefault(f => f.UserId == _user1.Id && f.FriendId == _user2.Id);

        Assert.That(acceptedRequest.Status, Is.EqualTo(FriendStatus.Friends));
    }

    [Test]
    public void TestFriendRequestCanBeDeclined()
    {
        // Create a new friend request
        var friendRequest = new Friend
        {
            UserId = _user1.Id,
            FriendId = _user2.Id,
            Status = FriendStatus.Pending
        };

        // Save it to database
        _dbContext.Friends.Add(friendRequest);
        _dbContext.SaveChanges();

        // Update status to Declined
        friendRequest.Status = FriendStatus.Declined;
        _dbContext.SaveChanges();

        // Check if status was updated
        var declinedRequest = _dbContext.Friends
            .FirstOrDefault(f => f.UserId == _user1.Id && f.FriendId == _user2.Id);

        Assert.That(declinedRequest.Status, Is.EqualTo(FriendStatus.Declined));
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}