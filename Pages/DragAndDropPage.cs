using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Pages;

public class DragAndDropPage
{
    private readonly IPage _page;

    // Heading
    private ILocator DragAndDropHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "Drag and Drop" });

    // Columns
    public ILocator ColumnA => _page.Locator("#column-a");
    public ILocator ColumnB => _page.Locator("#column-b");
    public ILocator ColumnAHeader => _page.Locator("#column-a header");
    public ILocator ColumnBHeader => _page.Locator("#column-b header");

    public DragAndDropPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync("https://the-internet.herokuapp.com/drag_and_drop");
    }

    // Drag column A onto column B
    public async Task DragColumnAOntoColumnBAsync()
    {
        await ColumnA.DragToAsync(ColumnB);
    }
}