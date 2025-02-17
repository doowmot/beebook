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
            var posts = dbContext.Posts // Fetches and lists a users own post by their user id 
                .Where(p => p.UserId == Id) 
                .ToList();

            // Retrieve Friends where the current user is either sender or receiver
            var friends = dbContext.Friends
                .Where(f => (f.UserId == Id && f.FriendId == loggedInUserId.Value) ||
                            (f.UserId == loggedInUserId.Value && f.FriendId == Id))
                .FirstOrDefault();

            // Determine Friendship Status 
            FriendStatus? friendStatus = null; // Default: null (meaning no status yet)
        
            if (friends != null)
            {
                friendStatus = friends.Status; // Use the actual status if a friendship exists
            }
            var model = new UserProfileViewModel
            {
                User = user,
                IsOwnProfile = loggedInUserId.Value == Id,
                Posts = posts,
                FriendStatus = friendStatus // New: Pass friendship status to the view
            };

            return View(model);
        }
    }
    
    [Route("/settings")]
    [HttpGet]
    public IActionResult Settings()
    {
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");

        if (!loggedInUserId.HasValue)
            {
                return RedirectToAction("New", "Sessions"); // Redirect to login if not logged in
            }
         using (AcebookDbContext dbContext = new AcebookDbContext()) // Creating a new instance of DbContext
        {
            var user = dbContext.Users?.Find(loggedInUserId);
            var model = new SettingsViewModel
            {
                User = user
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
