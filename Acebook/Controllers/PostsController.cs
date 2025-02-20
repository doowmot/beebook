using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;
using Npgsql.Internal;
using Microsoft.EntityFrameworkCore;

namespace acebook.Controllers;

[ServiceFilter(typeof(AuthenticationFilter))]
public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;

    public PostsController(ILogger<PostsController> logger)
    {
        _logger = logger;
    }

  [Route("/posts")]
  [HttpGet]
  public IActionResult Index()
  {
      AcebookDbContext dbContext = new AcebookDbContext();
      
      List<Post> posts = dbContext.Posts
          .Include(p => p.Comments)
          .ThenInclude(c => c.User)
          .OrderByDescending(p => p.CreatedAt)
          .ToList();

      ViewBag.Posts = posts;
      return View();
  }


    [Route("/posts")]
    [HttpPost]
    public RedirectResult Create(Post post) {
      AcebookDbContext dbContext = new AcebookDbContext();
      int currentUserId = HttpContext.Session.GetInt32("user_id").Value;
      post.UserId = currentUserId;
      
      dbContext.Posts.Add(post);
      dbContext.SaveChanges();
      return new RedirectResult("/posts");
    }

  [HttpPost]
  [Route("DeletePost")]
  public IActionResult DeletePost([FromBody] DeletePostRequest request)
  {
      try
      {
          if (request == null)
          {
              return BadRequest(new { success = false, message = "Request body is null." });
          }

          AcebookDbContext dbContext = new AcebookDbContext();

          // Get user from session
          int? userIdFromSession = HttpContext.Session.GetInt32("user_id");
          if (userIdFromSession == null)
          {
              return BadRequest(new { success = false, message = "User session not found. Please log in." });
          }
          int currentUserId = userIdFromSession.Value;

          // Get postId from request
          int postId = request.PostId;
          var post = dbContext.Posts.FirstOrDefault(p => p.Id == postId);

          if (post == null)
          {
              return BadRequest(new { success = false, message = "Post not found." });
          }

          if (post.UserId != currentUserId)
          {
              return BadRequest(new { success = false, message = "You can only delete your own posts." });
          }

          dbContext.Posts.Remove(post);
          dbContext.SaveChanges();

          return Ok(new { success = true });
      }
      catch (Exception ex)
      {
          // Log the error
          Console.WriteLine("Error during delete operation: " + ex.Message);

          // Return a 500 Internal Server Error with the exception message
          return StatusCode(500, new { success = false, message = "An error occurred while deleting the post.", error = ex.Message });
      }
  }






    [Route("/posts/comment")]
    [HttpGet]
    public IActionResult CommentsIndex(int postId) 
    {
      AcebookDbContext dbContext = new AcebookDbContext();
      List<Comment> comments = dbContext.Comments.ToList();
      ViewBag.Comments = comments;
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
