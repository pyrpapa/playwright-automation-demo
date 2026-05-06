using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Config;

namespace PlaywrightAutomationDemo.Tests;

[TestFixture]
public class ApiTests : PlaywrightTest
{
    private IAPIRequestContext _apiContext;

    // Class variables - same pattern as your work CRUD test
    private int _postId;
    private string? _postTitle;
    private string? _postBody;

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
    public async Task CRUD_CreatePost()
    {
        var response = await _apiContext.PostAsync("/posts", new APIRequestContextOptions
        {
            DataObject = new { title = "Test Post", body = "Test Body", userId = 1 }
        });

        Assert.That(response.Status, Is.EqualTo(201));

        var json = await response.JsonAsync();
        _postId = json.Value.GetProperty("id").GetInt32();
        _postTitle = json.Value.GetProperty("title").GetString();
        _postBody = json.Value.GetProperty("body").GetString();

        TestContext.Out.WriteLine($"Created Post ID: {_postId}");
        TestContext.Out.WriteLine($"Title: {_postTitle}");
        TestContext.Out.WriteLine($"Body: {_postBody}");
    }
}