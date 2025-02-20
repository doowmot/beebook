namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class UserLikesTests
{
    [Test]
    public void TestUserCanLikeAnotherUsersPost()
    {
        // SETUP DATABASE
        Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
        AcebookDbContext dbContext = new AcebookDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // SETUP USERS
        User user1 = new User("Test User 1", "test1@example.com", "password123");
        User user2 = new User("Test User 2", "test2@example.com", "password456");
        dbContext.Users.AddRange(user1, user2);
        dbContext.SaveChanges();

        // SETUP POST
        Post post = new Post
        {
            Content = "Post by user 1",
            UserId = user1.Id
        };
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // SETUP LIKE
        UserLike like = new UserLike
        {
            UserId = user2.Id,
            PostId = post.Id
        };
        dbContext.UserLikes.Add(like);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        UserLike savedLike = dbContext.UserLikes
            .FirstOrDefault(l => l.UserId == user2.Id && l.PostId == post.Id);
        
        Assert.That(savedLike, Is.Not.Null);
        Assert.That(savedLike.UserId, Is.EqualTo(user2.Id));
        Assert.That(savedLike.PostId, Is.EqualTo(post.Id));

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestLikeCannotBeCreatedWithoutUserId()
    {
        // SETUP DATABASE
        Environment.SetEnvironmentVariable("DATABASE_NAME", "acebook_csharp_test");
        AcebookDbContext dbContext = new AcebookDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // SETUP USER
        User user = new User("Test User", "test@example.com", "password123");
        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        // SETUP POST
        Post post = new Post
        {
            Content = "Test post",
            UserId = user.Id
        };
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // ATTEMPT TO CREATE LIKE WITHOUT USERID
        UserLike invalidLike = new UserLike
        {
            PostId = post.Id
        };

        // VERIFY RESULTS
        Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException>(() => {
            dbContext.UserLikes.Add(invalidLike);
            dbContext.SaveChanges();
        });

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }   
}