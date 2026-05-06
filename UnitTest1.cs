using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightAutomationDemo;

[TestFixture]
public class SmokeTests : PageTest
{
    [Test]
    public async Task HomePageLoads()
    {
        await Page.GotoAsync("https://the-internet.herokuapp.com");
        await Expect(Page).ToHaveTitleAsync("The Internet");
    }

    [Test]
    public async Task LoginPageVisible()
    {
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");
        await Expect(Page.Locator("h2")).ToContainTextAsync("Login Page");
    }

    [Test]
    public async Task SuccessfulLogin()
    {
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");
        await Page.FillAsync("#username", "tomsmith");
        await Page.FillAsync("#password", "SuperSecretPassword!");
        await Page.ClickAsync("button[type='submit']");
        await Expect(Page.Locator(".flash.success")).ToContainTextAsync("You logged into a secure area!");
    }
}