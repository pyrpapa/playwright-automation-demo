using Microsoft.Playwright;
using PlaywrightAutomationDemo.Config;

namespace PlaywrightAutomationDemo.Pages;

public class LoginPage
{
    private readonly IPage _page;

    // Element Library
    private ILocator UsernameField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Username" });
    private ILocator PasswordField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" });
    private ILocator LoginButton => _page.GetByRole(AriaRole.Button, new() { Name = " Login" });
    private ILocator SuccessMessage => _page.Locator(".flash.success");
    private ILocator ErrorMessage => _page.Locator(".flash.error");

    public LoginPage(IPage page)
    {
        _page = page;
    }

    // Actions
    public async Task NavigateAsync()
    {
        await _page.GotoAsync(TestConfig.UiBaseUrl + "/login");
    }

    public async Task LoginAsync(string username, string password)
    {
        await UsernameField.FillAsync(username);
        await PasswordField.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task<bool> IsLoginSuccessfulAsync()
    {
        return await SuccessMessage.IsVisibleAsync();
    }

    public async Task<bool> IsLoginFailedAsync()
    {
        return await ErrorMessage.IsVisibleAsync();
    }
}