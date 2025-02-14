using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;

namespace acebook.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [Route("/signup")]
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    [Route("/signup")]
    [HttpPost]
    public RedirectResult Create(User user) {
        AcebookDbContext dbContext = new AcebookDbContext();
        if (dbContext.Users != null)
        {
            dbContext.Users.Add(user);
        }
        else
        {
            // Handle the case where Users is null
            throw new InvalidOperationException("Users DbSet is null.");
        }
        dbContext.SaveChanges();
        return new RedirectResult("/signin");
    }
    
    [Route("/profile/{Id}")]
    [HttpGet]
    public IActionResult Profile(int Id)
    {
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");

        if (!loggedInUserId.HasValue)
        {
            return RedirectToAction("New", "Sessions"); // Redirect to login if not logged in
        }

        using (AcebookDbContext dbContext = new AcebookDbContext()) // Creating a new instance of DbContext
        {
            var user = dbContext.Users?.Find(Id);
            if (user == null)
            {
                return NotFound();
            }
            // var friends = dbContext.Friends
            //     .Where(f => f.UserId == Id)
            //     .Select(f => f.FriendUser)
            //     .ToList();

        // Fetch Posts (Assumes a 'Posts' table with a foreign key 'UserId')
            var posts = dbContext.Posts
                .Where(p => p.UserId == Id) // Retrieve posts by user (where Id in Users matches UserId in Posts)
                // .OrderByDescending(p => p.CreatedAt) // Show newest posts first
                .ToList();

            var model = new UserProfileViewModel
            {
                User = user,
                IsOwnProfile = loggedInUserId.Value == Id,
                // Friends = friends, 
                Posts = posts
            };

            return View(model);
        }
    }
    
    [Route("/settings")]
    [HttpGet]
    public IActionResult Settings()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
