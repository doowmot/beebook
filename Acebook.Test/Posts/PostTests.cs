namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class PostTests
{
    [Test]
    public void TestPostIsSavedToDatabase()
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
            Content = "This is a test post",
            UserId = user.Id
        };

        // TEST ACTIONS
        
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Post savedPost = dbContext.Posts
            .FirstOrDefault(p => p.UserId == user.Id);

        Assert.That(savedPost, Is.Not.Null,
            "Post should be found in the database");
        Assert.That(savedPost.Content, Is.EqualTo("This is a test post"),
            "Post content should match what was saved");
        Assert.That(savedPost.UserId, Is.EqualTo(user.Id),
            "Post should be associated with correct user");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestPostCanBeDeletedFromDatabase()
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
            Content = "This post will be deleted",
            UserId = user.Id
        };
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // TEST ACTIONS
        dbContext.Posts.Remove(post);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Post deletedPost = dbContext.Posts
            .FirstOrDefault(p => p.UserId == user.Id);

        Assert.That(deletedPost, Is.Null,
            "Post should no longer exist in the database");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestPostsBelongToCorrectUser()
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

        // SETUP POSTS
        Post post1 = new Post
        {
            Content = "Post by user 1",
            UserId = user1.Id
        };
        Post post2 = new Post
        {
            Content = "Post by user 2",
            UserId = user2.Id
        };
        dbContext.Posts.AddRange(post1, post2);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Post user1Post = dbContext.Posts
            .FirstOrDefault(p => p.UserId == user1.Id);
        Post user2Post = dbContext.Posts
            .FirstOrDefault(p => p.UserId == user2.Id);

        Assert.That(user1Post.UserId, Is.EqualTo(user1.Id),
            "First post should belong to user 1");
        Assert.That(user2Post.UserId, Is.EqualTo(user2.Id),
            "Second post should belong to user 2");
        Assert.That(user1Post.Content, Is.EqualTo("Post by user 1"),
            "Content should match user 1's post");
        Assert.That(user2Post.Content, Is.EqualTo("Post by user 2"),
            "Content should match user 2's post");

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

}