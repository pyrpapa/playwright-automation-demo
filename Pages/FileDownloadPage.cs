using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Tests;

public class FileDownloadPage
{
    private readonly IPage _page;

    public FileDownloadPage(IPage page)
    {
        _page = page;
    }

    private ILocator LinkFor(string fileName) =>
        _page.GetByRole(AriaRole.Link, new() { Name = fileName });

    public async Task NavigateAsync()
    {
        await _page.GotoAsync("https://the-internet.herokuapp.com/download");
    }

    // the-internet.herokuapp.com's /download page just lists whatever is in the same
    // server-side folder that /upload writes into. That folder is shared across every
    // automated test suite that hits this public demo site and isn't guaranteed to
    // persist (the dyno's filesystem resets periodically), so tests can't assume any
    // particular file is already sitting there. Uploading here makes the fixture
    // self-contained instead of depending on fragile external/shared state.
    public async Task UploadFileAsync(string filePath)
    {
        await _page.GotoAsync("https://the-internet.herokuapp.com/upload");
        await _page.SetInputFilesAsync("#file-upload", filePath);
        await _page.ClickAsync("#file-submit");
    }

    public async Task<bool> IsFileLinkVisibleAsync(string fileName)
    {
        return await LinkFor(fileName).IsVisibleAsync();
    }

    // Click a named file link on the /download page and verify the download completes
    public async Task<string> ClickFileAndVerifyDownloadAsync(string fileName)
    {
        var downloadTask = _page.WaitForDownloadAsync();
        await LinkFor(fileName).ClickAsync();
        var download = await downloadTask;
        var path = await download.PathAsync();
        return path ?? string.Empty;
    }
}
