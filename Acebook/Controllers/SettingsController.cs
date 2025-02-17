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
                User = user
            };
            return View(model);
        }
    }  
    [Route("/settings")]
    [HttpPost]
    
    public async Task<IActionResult> Edit(User user)
    {
        // Check if the user exists in the database
        int? loggedInUserId = HttpContext.Session.GetInt32("user_id");
        AcebookDbContext dbContext = new AcebookDbContext();
        var loggedInUser = dbContext.Users?.Find(loggedInUserId);
        if (loggedInUser == null)
        {
            return NotFound("User not found");
        }
        else
        {
        // Update data
        // user.Name = user.NewName;
        // user.Email = user.NewEmail;
        //user.profile_picture = model.Profile_picture;
        }

        // Save changes to the database
        try
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return Ok("Data updated successfully");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Users DbSet is null.");
            // return StatusCode(500, "Internal server error: " + ex.Message);
        }
        // return View("Settings", model);
    }
}