using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Tests;

public class FileDownloadPage
{
    private readonly IPage _page;

    // File download links
    private ILocator SampleUploadTxtLink => _page.GetByRole(AriaRole.Link, new() { Name = "sample_upload.txt" });
    private ILocator NonExistentFileLink => _page.GetByRole(AriaRole.Link, new() { Name = "non_existent_file.txt" });

    public FileDownloadPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync("https://the-internet.herokuapp.com/download");
    }

    // Click "sample_upload.txt" and verify that file download completes
    public async Task<string> ClickSampleUploadTxtAndVerifyDownloadAsync()
    {
        var downloadTask = _page.WaitForDownloadAsync();
        await SampleUploadTxtLink.ClickAsync();
        var download = await downloadTask;
        var path = await download.PathAsync();
        return path ?? string.Empty;
    }

}
