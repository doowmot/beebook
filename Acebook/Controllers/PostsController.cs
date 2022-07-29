using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using acebook.ActionFilters;

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
    public IActionResult Index() {
      PostContext postContext = new PostContext();
      List<Post> posts = postContext.Posts.ToList();
      ViewBag.Posts = posts;
      return View();
    }

    [Route("/posts")]
    [HttpPost]
    public RedirectResult Create(Post post) {
      int user_id = HttpContext.Session.GetInt32("user_id").Value;
      System.Console.WriteLine("------");
      System.Console.WriteLine(user_id);
      System.Console.WriteLine("------");
      post.UserId = user_id;
      System.Console.WriteLine("------");
      System.Console.WriteLine(post.UserId);
      System.Console.WriteLine("------");
      PostContext postContext = new PostContext();
      postContext.Posts.Add(post);
      postContext.SaveChanges();
      return new RedirectResult("/posts");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
