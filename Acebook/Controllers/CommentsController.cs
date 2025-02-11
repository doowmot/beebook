using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;

namespace acebook.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ILogger<CommentsController> logger)
    {
        _logger = logger;
    }


    // [Route("/comments")]
    // [HttpPost]
    // public RedirectResult Create(int postId, Comment comment) {
    //   AcebookDbContext dbContext = new AcebookDbContext();
    //   int currentUserId = HttpContext.Session.GetInt32("user_id").Value;
    // //   comment = currentUserId;
    //   dbContext.Comment.Add(comment);
    //   dbContext.SaveChanges();
    //   return new RedirectResult("/posts");
    // }
    [Route("/comments")]
    [HttpPost]
    public RedirectResult Create(int postId, string commentContent)
    {
        AcebookDbContext dbContext = new AcebookDbContext();
        int currentUserId = HttpContext.Session.GetInt32("user_id").Value;

        Comment comment = new Comment
        {
            Comments = commentContent,
            PostId = postId,   // Associate comment with the correct post
            UserId = currentUserId,
            DateTime = DateTime.UtcNow
        };

        dbContext.Comments.Add(comment);
        dbContext.SaveChanges();
        
        return new RedirectResult("/posts");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
