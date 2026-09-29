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
        await _loginPage.AssertLoginSuccessfulAsync();
    }

    [Test]
    public async Task FailedLoginWithIncorrectPassword()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("tomsmith", "wrongpassword");
        await _loginPage.AssertLoginUnsuccessfulAsync();
    }

    [Test]
    public async Task FailedLoginWithIncorrectUsername()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("wronguser", "SuperSecretPassword!");
        await _loginPage.AssertLoginUnsuccessfulAsync();
    }
}