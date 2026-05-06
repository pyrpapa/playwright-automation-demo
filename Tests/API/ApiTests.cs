using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Config;
using Newtonsoft.Json;
using FluentAssertions;
using PlaywrightAutomationDemo.Models;


namespace PlaywrightAutomationDemo.Tests.API;

[TestFixture]
public class ApiTests : PlaywrightTest
{
    private IAPIRequestContext _apiContext;

    // Known data from JSONPlaceholder
    private const int PostId = 1;
    private const int UserId = 1;
    private const string PostTitle = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit";
    private const string PostBody = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto";

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
    public async Task CRUD_Post()
    {
        // CREATE
        var createResponse = await _apiContext.PostAsync("/posts", new APIRequestContextOptions
        {
            DataObject = new { title = "Test Post", body = "Test Body", userId = 1 }
        });
        var createBody = await createResponse.TextAsync(); 
        Console.WriteLine($"POST response body: {createBody}");
        Assert.That(createResponse.Status, Is.EqualTo(201));


        // READ
        var getResponse = await _apiContext.GetAsync($"/posts/{PostId}");
        var getBody = await getResponse.TextAsync(); 
        Console.WriteLine($"GET response body: {getBody}");
        Assert.That(getResponse.Status, Is.EqualTo(200));

        var post = JsonConvert.DeserializeObject<Post>(getBody);
        // Schema validation - field exists and has a value
        post!.Id.Should().NotBe(0);
        post.Title.Should().NotBeNullOrEmpty();

        // Value validation - field matches what we expect
        post.Id.Should().Be(PostId);
        post.Title.Should().Be(PostTitle);

        // UPDATE
        var updateResponse = await _apiContext.PutAsync($"/posts/{PostId}", new APIRequestContextOptions
        {
            DataObject = new { title = "Updated Title", body = "Updated Body", userId = 1 }
        });
        var updateBody = await updateResponse.TextAsync(); 
        Console.WriteLine($"PUT response body: {updateBody}");
        Assert.That(updateResponse.Status, Is.EqualTo(200));
        var updateJson = await updateResponse.JsonAsync();
        Assert.That(updateJson.Value.GetProperty("title").GetString(), Is.EqualTo("Updated Title"));
        Assert.That(updateJson.Value.GetProperty("body").GetString(), Is.EqualTo("Updated Body"));


        // DELETE
        var deleteResponse = await _apiContext.DeleteAsync($"/posts/{PostId}");
        var deleteBody = await deleteResponse.TextAsync();
        Console.WriteLine($"DELETE response body: {deleteBody}");
        Assert.That(deleteResponse.Status, Is.EqualTo(200));
    }
}