using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using acebook.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace acebook.Controllers;

public class SettingsController : Controller
{
    private readonly ILogger<UsersController> _logger;

    public SettingsController(ILogger<UsersController> logger)
    {
        _logger = logger;
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
                User = user,
                ProfilePicturePath = user.ProfilePicturePath
            };
            return View(model);
        }
    }  
    [Route("/settings")]
    [HttpPost]
    
    public async Task<IActionResult> Edit(SettingsViewModel model)
    {
        // Check if the user exists in the database
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");
        AcebookDbContext dbContext = new AcebookDbContext();
        var loggedInUser = dbContext.Users?.Find(loggedInUserId);
        if (loggedInUser == null)
        {
            return NotFound("User not found");
        }
        
        // Update data
            loggedInUser.Name = model.Name;
            loggedInUser.Email = model.Email;
            loggedInUser.ProfilePicturePath = model.ProfilePicturePath??"https://images.unsplash.com/photo-1568526381923-caf3fd520382?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

        // Save changes to the database
        try
        {
            dbContext.Users.Update(loggedInUser);
            await dbContext.SaveChangesAsync();
            return View();
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Settings not updated.");
        }
    }
}