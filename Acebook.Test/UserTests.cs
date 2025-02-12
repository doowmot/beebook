namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;

public class UserTests
{
    [Test]
    public void FriendIsAddedToFriendList()
    {
        User user1 = new User("User1", "user1@example.com", "password123");
        User user2 = new User("User2", "user2@example.com", "password456");

        user1.AddFriend(user2);
        
        Assert.That(user1.Friends.Contains(user2));
    }

} 