using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using PlaywrightAutomationDemo.Pages;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Tests;

[TestFixture]
public class DragAndDropTests : PageTest
{
    private DragAndDropPage _dragAndDropPage;

    [SetUp]
    public async Task SetUp()
    {
        _dragAndDropPage = new DragAndDropPage(Page);
        await _dragAndDropPage.NavigateAsync();
    }

    [Test]
    public async Task DragAndDrop_PageLoads_HeadingIsVisible()
    {
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Drag and Drop" }))
            .ToBeVisibleAsync();
    }

    [Test]
    public async Task DragAndDrop_InitialState_ColumnAIsOnLeft()
    {
        await Expect(_dragAndDropPage.ColumnAHeader).ToHaveTextAsync("A");
        await Expect(_dragAndDropPage.ColumnBHeader).ToHaveTextAsync("B");
    }

    [Test]
    public async Task DragAndDrop_DragAOntoB_ColumnsSwap()
    {
        await _dragAndDropPage.DragColumnAOntoColumnBAsync();

        await Expect(_dragAndDropPage.ColumnAHeader).ToHaveTextAsync("B");
        await Expect(_dragAndDropPage.ColumnBHeader).ToHaveTextAsync("A");
    }

    [Test]
    public async Task DragAndDrop_DragAOntoB_ThenDragBack_ColumnsRestored()
    {
        await _dragAndDropPage.DragColumnAOntoColumnBAsync();
        await _dragAndDropPage.DragColumnAOntoColumnBAsync();

        await Expect(_dragAndDropPage.ColumnAHeader).ToHaveTextAsync("A");
        await Expect(_dragAndDropPage.ColumnBHeader).ToHaveTextAsync("B");
    }
}