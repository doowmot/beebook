namespace Acebook.Test;

using NUnit.Framework;
using acebook.Models;

public class UserTests
{
    [Test]
    public void TestFriendIsAddedToFriendList()
    // Testing adding a new friend to our friend list
    {
        User user1 = new User("User1", "user1@example.com", "password123");
        User user2 = new User("User2", "user2@example.com", "password456");

        user1.AddFriend(user2);
        Assert.That(user1.Friends.Contains(user2));
    }

    [Test]
    public void TestCannotAddDuplicateFriends()
    // Testing that the user cannot add the same friend twice
    {
        User user1 = new User("User1", "user1@example.com", "password123");
        User user2 = new User("User2", "user2@example.com", "password456");

        user1.AddFriend(user2);
        Assert.That(user1.Friends.Contains(user2));

        string result = user1.AddFriend(user2);
        Assert.That(result, Is.EqualTo("Error: This person is already a friend"));
    }

    [Test]
    public void TestFriendIsRemovedFromFriendList()
    // Testing that the user can remove a friend from their friend list
    {
        User user1 = new User("User1", "user1@example.com", "password123");
        User user2 = new User("User2", "user2@example.com", "password456");

        user1.AddFriend(user2);
        Assert.That(user1.Friends.Contains(user2));
        user1.RemoveFriend(user2);
        Assert.That(user1.Friends.Contains(user2), Is.False);
    }

    [Test]
    public void TestCannotRemoveFriendThatIsNotAFriend()
    // Testing that the user cannot remove a friend that they are not friends with
    {
        User user1 = new User("User1", "user1@example.com", "password123");
        User user2 = new User("User2", "user2@example.com", "password456");

        string result = user1.RemoveFriend(user2);
        Assert.That(result, Is.EqualTo("Error: Cannot remove this person as they are not a friend"));
    }

}