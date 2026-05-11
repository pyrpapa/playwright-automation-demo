using Microsoft.Playwright;
using PlaywrightAutomationDemo.Config;


namespace PlaywrightAutomationDemo.Pages;

public class BasicAuthPage
{
    private readonly IPage _page;
    private ILocator SuccessMessage => _page.Locator("p").Filter(new() { HasText = "Congratulations" });

    public BasicAuthPage(IPage page)
    {
        _page = page;
    }

    public async Task<bool> BasicAuthSuccessAsync()
    {
        await _page.GotoAsync("https://admin:admin@the-internet.herokuapp.com/basic_auth");
        return await SuccessMessage.IsVisibleAsync();
    }

    public async Task<bool> BasicAuthFailureAsync()
    {
        await _page.GotoAsync("https://wronguser:wrongpass@the-internet.herokuapp.com/basic_auth");
        return await SuccessMessage.IsVisibleAsync();
    }
}