using Microsoft.Playwright;
using PlaywrightAutomationDemo.Config;
namespace PlaywrightAutomationDemo.Pages;

public class CheckboxPage
{
    private readonly IPage _page;

    // Locators
    private ILocator FirstCheckbox => _page.Locator("input[type='checkbox']").Nth(0);
    private ILocator SecondCheckbox => _page.Locator("input[type='checkbox']").Nth(1);

    public CheckboxPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync(TestConfig.UiBaseUrl + "/checkboxes");
    }

    public async Task ClickFirstCheckboxAsync()
    {
        await FirstCheckbox.ClickAsync();
    }

    public async Task ClickSecondCheckboxAsync()
    {
        await SecondCheckbox.ClickAsync();
    }

    public async Task<bool> IsFirstCheckedAsync()
    {
        return await FirstCheckbox.IsCheckedAsync();
    }

    public async Task<bool> IsSecondCheckedAsync()
    {
        return await SecondCheckbox.IsCheckedAsync();
    }
}