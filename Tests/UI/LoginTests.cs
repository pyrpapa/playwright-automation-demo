using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Pages;

namespace PlaywrightAutomationDemo.Tests.UI;

[TestFixture]
public class LoginTests : PageTest
{
    private LoginPage _loginPage;

    [SetUp]
    public void SetUp()
    {
        _loginPage = new LoginPage(Page);
    }

    [Test]
    public async Task SuccessfulLogin()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("tomsmith", "SuperSecretPassword!");
        Assert.That(await _loginPage.IsLoginSuccessfulAsync(), Is.True);
    }

    [Test]
    public async Task FailedLoginWithBadPassword()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("tomsmith", "wrongpassword");
        Assert.That(await _loginPage.IsLoginFailedAsync(), Is.True);
    }

    [Test]
    public async Task FailedLoginWithBadUsername()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("wronguser", "SuperSecretPassword!");
        Assert.That(await _loginPage.IsLoginFailedAsync(), Is.True);
    }
}