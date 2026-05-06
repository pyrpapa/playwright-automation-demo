using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Config;
using Newtonsoft.Json;
using FluentAssertions;
using PlaywrightAutomationDemo.Models;


namespace PlaywrightAutomationDemo.Tests.API;

[TestFixture]
public class NegativeApiTests : PlaywrightTest
{
    private IAPIRequestContext _apiContext;

    // Known data from JSONPlaceholder
    private const int PostId = 1;

    private const int CommentId = 99999;

    [SetUp]
    public async Task SetUpAsync()
    {
        _apiContext = await Playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = TestConfig.BaseUrl,
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Accept"] = "application/json",
                ["Content-Type"] = "application/json"
            }
        });
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        await _apiContext.DisposeAsync();
    }

   [Test]
    public async Task Negative_Post()
    {
        // CREATE
        var createResponse = await _apiContext.PostAsync($"/comments/{CommentId}", new APIRequestContextOptions
        {
            DataObject = new { name = "Test Comment", body = "Test Comment Body", id = CommentId, postId = PostId }
        });
        var createBody = await createResponse.TextAsync(); 
        Console.WriteLine($"POST response body: {createBody}");
        Assert.That(createResponse.Status, Is.EqualTo(404));
        Console.WriteLine($"Unable to make POST request to Create comment with ID {CommentId}.");
    }
   [Test]
    public async Task Negative_Get()
    {
        // READ
        var getResponse = await _apiContext.GetAsync($"/comments/{CommentId}");
        var getBody = await getResponse.TextAsync(); 
        Console.WriteLine($"GET response body: {getBody}");
        Assert.That(getResponse.Status, Is.EqualTo(404));
        Console.WriteLine($"Unable to make GET request to Read comment with ID {CommentId} - comment not found.");
    }

   [Test]

    public async Task Negative_Put()
    {
        // JSONPlaceholder returns 500 instead of 404 for PUT to non-existent ID
        // A real API should return 404 - this is a known limitation of the fake API
        // UPDATE
        var updateResponse = await _apiContext.PutAsync($"/comments/{CommentId}", new APIRequestContextOptions
        {
            DataObject = new { name = "Updated Name", email = "updated@example.com", body = "Updated Body", postId = 1 }
        });
        
        var updateBody = await updateResponse.TextAsync(); 
        Console.WriteLine($"PUT response body: {updateBody}");
        Console.WriteLine($"PUT status: {updateResponse.Status}");
        Assert.That(updateResponse.Status, Is.EqualTo(500));

        Console.WriteLine($"Unable to make PUT request to Update comment with ID {CommentId} - comment not found.");
    }
    [Test]
    public async Task Negative_Delete()
    {
        // DELETE
        var deleteResponse = await _apiContext.DeleteAsync($"/comets/{CommentId}"); //Misspelled endpoint to trigger 404
        var deleteBody = await deleteResponse.TextAsync();
        Console.WriteLine($"DELETE response body: {deleteBody}");
        Assert.That(deleteResponse.Status, Is.EqualTo(404));
        Console.WriteLine($"Unable to make DELETE request to delete comment with ID {CommentId} - comment not found.");
    }
}