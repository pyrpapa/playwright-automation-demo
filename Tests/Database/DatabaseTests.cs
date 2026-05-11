using NUnit.Framework;
using FluentAssertions;
using PlaywrightAutomationDemo.Helpers;
using PlaywrightAutomationDemo.Models;

namespace PlaywrightAutomationDemo.Tests.Database;

[TestFixture]
public class DatabaseTests
{
    [OneTimeSetUp]
    public void InitializeDatabase()
    {
        DatabaseHelper.Initialize();
    }

    [SetUp]
    public void CleanupBeforeEach()
    {
        DatabaseHelper.Cleanup();
    }

    [Test]
    public void DB_CreatePost()
    {
        DatabaseHelper.SeedPost(1, "Test Title", "Test Body", 1);

        var post = DatabaseHelper.GetPost(1);
        post.Should().NotBeNull();
        post!.Title.Should().Be("Test Title");
        post.Body.Should().Be("Test Body");
        post.UserId.Should().Be(1);

        Console.WriteLine($"Created Post ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"UserId: {post.UserId}");
    }

    [Test]
    public void DB_ReadPost()
    {
        DatabaseHelper.SeedPost(2, "Read Title", "Read Body", 1);

        var post = DatabaseHelper.GetPost(2);
        post.Should().NotBeNull();
        post!.Id.Should().Be(2);
        post.Title.Should().Be("Read Title");

        Console.WriteLine($"Read Post ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
    }

    [Test]
    public void DB_DeletePost()
    {
        DatabaseHelper.SeedPost(3, "Delete Title", "Delete Body", 1);
        DatabaseHelper.DeletePost(3);

        var post = DatabaseHelper.GetPost(3);
        post.Should().BeNull();

        Console.WriteLine($"Post with ID 3 successfully deleted - returned null: {post == null}");

    }

    [Test]
    public void DB_ReadNonExistent()
    {
        var post = DatabaseHelper.GetPost(99999);
        post.Should().BeNull();

        Console.WriteLine($"Success: Post with ID 99999 does not exist - returned null: {post == null}");

    }
}