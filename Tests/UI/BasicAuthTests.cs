using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Pages;

namespace PlaywrightAutomationDemo.Tests.UI;

[TestFixture]
public class BasicAuthTests : PageTest
{
    private BasicAuthPage _basicAuthPage;

    [SetUp]
    public void SetUp()
    {
        _basicAuthPage = new BasicAuthPage(Page);
    }

    [Test]
    public async Task BasicAuth_Success()
    {
        Assert.That(await _basicAuthPage.BasicAuthSuccessAsync(), Is.True);
        Console.WriteLine($"Basic authentication successful with correct credentials.");
    }

    [Test]
    public async Task BasicAuth_Failure()
    {
        Assert.That(await _basicAuthPage.BasicAuthFailureAsync(), Is.False);
        Console.WriteLine($"Basic authentication failed with incorrect credentials.");
    }
}