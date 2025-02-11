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

    // [Route("/posts")]
    // [HttpGet]
    // public IActionResult Index() {
    //   AcebookDbContext dbContext = new AcebookDbContext();
    //   List<Post> posts = dbContext.Posts.ToList();
    //   ViewBag.Posts = posts;
    //   return View();
    // }

  [Route("/posts")]
  [HttpGet]
  public IActionResult Index()
  {
      AcebookDbContext dbContext = new AcebookDbContext();
      
      List<Post> posts = dbContext.Posts
          .Include(p => p.Comments) // Include related comments
          .ThenInclude(c => c.User) // Include user info for comments
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

    [Route("/posts/comment")]
    [HttpGet]
    public IActionResult CommentsIndex(int postId) 
    {
      AcebookDbContext dbContext = new AcebookDbContext();
      List<Comment> comments = dbContext.Comments.ToList();
      ViewBag.Comments = comments;
      return View();
    }

    // [Route("/comment/create")]
    // [HttpPost]
    // public RedirectResult CommentCreate(int postId, Comment comment) 
    // {
    //   AcebookDbContext dbContext = new AcebookDbContext();
    //   int currentUserId = HttpContext.Session.GetInt32("user_id").Value;
    //   post.UserId = currentUserId;
    //   dbContext.Posts.Add(comment);
    //   dbContext.SaveChanges();
    //   return new RedirectResult("/posts");
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
