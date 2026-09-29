using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using FluentAssertions;
using Newtonsoft.Json;
using PlaywrightAutomationDemo.Config;
using PlaywrightAutomationDemo.Helpers;
using PlaywrightAutomationDemo.Models;

namespace PlaywrightAutomationDemo.Tests.API;

[TestFixture]
public class ApiToDbReconciliation : PlaywrightTest
{
    private IAPIRequestContext _apiContext = null!;

    [OneTimeSetUp]
    public void InitializeDatabase()
    {
        DatabaseHelper.Initialize();
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        DatabaseHelper.Cleanup();
        _apiContext = await Playwright.APIRequest.NewContextAsync(new() 
        { 
            BaseURL= TestConfig.BaseUrl, 
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Accept"] = "application/json",
                ["Content-Type"] = "application/json"
            }
        });
    }
    


    [TearDown]
    public async Task TearDownAsync() => await _apiContext.DisposeAsync();

   [Test]
    public async Task Api_To_Db_Reconciliation()
    {
        // Arrange: get the source of truth from the API
        var response = await _apiContext.GetAsync("/posts");
        Assert.That(response.Status, Is.EqualTo(200));

        var apiPosts = JsonConvert.DeserializeObject<List<Post>>(await response.TextAsync())!;
        Assert.That(apiPosts, Has.Count.EqualTo(100), "API did not return the expected 100 posts");

        // Act: load it into the database
        foreach (var p in apiPosts)
            DatabaseHelper.SeedPost(p.Id, p.Title, p.Body, p.UserId);


        // Assert: reconcile database against API
        var dbCount         = DatabaseHelper.CountPosts();
        var dupes           = DatabaseHelper.CountDuplicateIds();
        var noTitle         = DatabaseHelper.CountMissingTitles();
        var apiPost1        = apiPosts.Single(p => p.Id == 1);
        var dbPost1         = DatabaseHelper.GetPost(1)!;
        var apiCountsByUser = apiPosts.GroupBy(p => p.UserId)
                              .ToDictionary(g => g.Key, g => g.Count());
        var dbCountsByUser  = DatabaseHelper.GetPostCountsByUser();

        TestContext.Out.WriteLine($"Users: API {apiCountsByUser.Count} | DB {dbCountsByUser.Count}");
        TestContext.Out.WriteLine($"API posts: {apiPosts.Count} | DB rows: {dbCount}");
        TestContext.Out.WriteLine($"Duplicate Ids: {dupes}");
        TestContext.Out.WriteLine($"Missing Titles: {noTitle}");
        TestContext.Out.WriteLine($"Spot check post 1: '{dbPost1.Title}'");
        
        Assert.Multiple(() =>
        {
            Assert.That(dbCount, Is.EqualTo(apiPosts.Count),
                $"Row count mismatch: API {apiPosts.Count}, DB {dbCount}");
            Assert.That(dupes, Is.EqualTo(0), $"Found {dupes} duplicate Ids");
            Assert.That(noTitle, Is.EqualTo(0), $"Found {noTitle} entries without Title");
            Assert.That(dbPost1.UserId, Is.EqualTo(apiPost1.UserId), "Post 1 UserId");
            Assert.That(dbPost1.Title,  Is.EqualTo(apiPost1.Title),  "Post 1 Title");
            Assert.That(dbPost1.Body,   Is.EqualTo(apiPost1.Body),   "Post 1 Body");
            //Equivalent To compares lists, ignoring order
            Assert.That(dbCountsByUser, Is.EquivalentTo(apiCountsByUser),
                "Post counts per user do not match between API and DB");
        });
    }
}