using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class LikesController : Controller
{
    private readonly ILogger<LikesController> _logger;

    public LikesController(ILogger<LikesController> logger)
    {
        _logger = logger;
    }

[HttpPost]
[Route("IncrementLike")]
public IActionResult Like(int postId)
{
    AcebookDbContext dbContext = new AcebookDbContext();
    int currentUserId = HttpContext.Session.GetInt32("user_id").Value;

    var post = dbContext.Posts.FirstOrDefault(p => p.Id == postId);
    if (post != null && post.UserId != currentUserId)
    {
        var userLike = dbContext.UserLikes.FirstOrDefault(ul => ul.UserId == currentUserId && ul.PostId == postId);
        if (userLike == null)
        {
            post.LikesCount++;
            dbContext.Posts.Update(post);

            var newLike = new UserLike { UserId = currentUserId, PostId = postId };
            dbContext.UserLikes.Add(newLike);

            dbContext.SaveChanges();

            // Return a success response
            return Ok(post.LikesCount);
        } 
        else
            {
            // Return an empty success response if the user has already liked the post
            return Ok(post.LikesCount);
            }
    }
    else
    {
        // Return an empty success response if the user is trying to like their own post
    return Ok(post.LikesCount);
    }
}

}

