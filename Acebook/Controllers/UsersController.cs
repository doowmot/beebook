using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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
            user.ProfilePicturePath = "https://images.unsplash.com/photo-1568526381923-caf3fd520382?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";
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
    
    
    [Route("/notifications")]
    [HttpGet]
    public IActionResult Notifications()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
