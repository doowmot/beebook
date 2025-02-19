namespace acebook.Models; // Make sure this matches your project namespace

public class SettingsViewModel
    {
        public User User { get; set; }  // User profile data
        
        
        public string Name {get; set;}
        public string Email { get; set; }    
        public string ProfilePicturePath {get; set;}
    }


