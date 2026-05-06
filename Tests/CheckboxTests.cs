using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Pages;

namespace PlaywrightAutomationDemo.Tests;

[TestFixture]
public class CheckboxTests : PageTest
{
    private CheckboxPage  _checkboxPage;
        
    [SetUp]
    public void SetUp()
    {
        _checkboxPage = new CheckboxPage(Page);
    }

    [Test]
    public async Task ToggleFirstCheckbox()
    {
        await _checkboxPage.NavigateAsync();
        
        bool initialState = await _checkboxPage.IsFirstCheckedAsync();
        await _checkboxPage.ClickFirstCheckboxAsync();
        Assert.That(await _checkboxPage.IsFirstCheckedAsync(), Is.EqualTo(!initialState));
    }

    [Test]
    public async Task CheckBothCheckboxes()
    {
        await _checkboxPage.NavigateAsync();

        if (!await _checkboxPage.IsFirstCheckedAsync())
            await _checkboxPage.ClickFirstCheckboxAsync();

        if (!await _checkboxPage.IsSecondCheckedAsync())
            await _checkboxPage.ClickSecondCheckboxAsync();

        TestContext.Out.WriteLine("First checked: " + await _checkboxPage.IsFirstCheckedAsync());
        TestContext.Out.WriteLine("Second checked: " + await _checkboxPage.IsSecondCheckedAsync());

        Assert.That(await _checkboxPage.IsFirstCheckedAsync(), Is.True);
        Assert.That(await _checkboxPage.IsSecondCheckedAsync(), Is.True);
    }
    [Test]
    public async Task UncheckBothCheckboxes()
    {
        await _checkboxPage.NavigateAsync();

        if (await _checkboxPage.IsFirstCheckedAsync())
            await _checkboxPage.ClickFirstCheckboxAsync();

        if (await _checkboxPage.IsSecondCheckedAsync())
            await _checkboxPage.ClickSecondCheckboxAsync();

        TestContext.Out.WriteLine("First unchecked: " + await _checkboxPage.IsFirstCheckedAsync());
        TestContext.Out.WriteLine("Second unchecked: " + await _checkboxPage.IsSecondCheckedAsync());

        Assert.That(await _checkboxPage.IsFirstCheckedAsync(), Is.False);
        Assert.That(await _checkboxPage.IsSecondCheckedAsync(), Is.False);
    }
}