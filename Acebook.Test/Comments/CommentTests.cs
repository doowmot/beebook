namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;
using Microsoft.EntityFrameworkCore;

public class CommentTests
{
    [Test]
    public void TestCommentIsSavedToDatabase()
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
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // SETUP COMMENT
        Comment comment = new Comment
        {
            Comments = "This is a test comment",
            UserId = user.Id,
            PostId = post.Id,
            DateTime = DateTime.UtcNow
        };

        // TEST ACTIONS
        dbContext.Comments.Add(comment);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Comment savedComment = dbContext.Comments
            .FirstOrDefault(c => c.UserId == user.Id);

        Assert.That(savedComment, Is.Not.Null);
        Assert.That(savedComment.Comments, Is.EqualTo("This is a test comment"));
        Assert.That(savedComment.UserId, Is.EqualTo(user.Id));
        Assert.That(savedComment.PostId, Is.EqualTo(post.Id));

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    [Test]
    public void TestCommentCanBeDeletedFromDatabase()
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
        dbContext.Posts.Add(post);
        dbContext.SaveChanges();

        // SETUP COMMENT
        Comment comment = new Comment
        {
            Comments = "This is a test comment",
            UserId = user.Id,
            PostId = post.Id,
            DateTime = DateTime.UtcNow
        };

        // SAVE COMMENT
        dbContext.Comments.Add(comment);
        dbContext.SaveChanges();

        // VERIFY COMMENT WAS SAVED
        Comment savedComment = dbContext.Comments
            .FirstOrDefault(c => c.UserId == user.Id);
        Assert.That(savedComment, Is.Not.Null);

        // DELETE COMMENT
        dbContext.Comments.Remove(comment);
        dbContext.SaveChanges();

        // VERIFY COMMENT WAS DELETED
        Comment deletedComment = dbContext.Comments
            .FirstOrDefault(c => c.UserId == user.Id);
        Assert.That(deletedComment, Is.Null);

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

      [Test]
    public void TestCommentsBelongToCorrectUserAndPost()
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

        // SETUP COMMENTS
        Comment comment1 = new Comment
        {
            Comments = "Comment by user 1 on post 2",
            UserId = user1.Id,
            PostId = post2.Id,
            DateTime = DateTime.UtcNow
        };
        Comment comment2 = new Comment
        {
            Comments = "Comment by user 2 on post 1",
            UserId = user2.Id,
            PostId = post1.Id,
            DateTime = DateTime.UtcNow
        };
        dbContext.Comments.AddRange(comment1, comment2);
        dbContext.SaveChanges();

        // VERIFY RESULTS
        Comment user1Comment = dbContext.Comments
            .FirstOrDefault(c => c.UserId == user1.Id);
        Comment user2Comment = dbContext.Comments
            .FirstOrDefault(c => c.UserId == user2.Id);

        Assert.That(user1Comment.UserId, Is.EqualTo(user1.Id));
        Assert.That(user1Comment.PostId, Is.EqualTo(post2.Id));
        Assert.That(user2Comment.UserId, Is.EqualTo(user2.Id));
        Assert.That(user2Comment.PostId, Is.EqualTo(post1.Id));

        // CLEANUP
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }






}