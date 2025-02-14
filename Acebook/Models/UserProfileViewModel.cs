namespace acebook.Models // Make sure this matches your project namespace
{
    public class UserProfileViewModel
    {
        public User User { get; set; }  // User profile data
        public bool IsOwnProfile { get; set; } // True if viewing own profile, false if viewing another user's
        public List<Post> Posts { get; set; } = new List<Post>();   // List of posts
        public List<Post> PostsController { get; internal set; }
        // public List<User> Friends { get; set; } = new List<User>(); // List of friends
    }
}
