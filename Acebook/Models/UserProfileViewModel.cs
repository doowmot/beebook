namespace acebook.Models // Make sure this matches your project namespace
{
    public class UserProfileViewModel
    {
        public User User { get; set; }  // Represents the user profile data
        public bool IsOwnProfile { get; set; } // True if viewing own profile, false if viewing another user's
    }
}