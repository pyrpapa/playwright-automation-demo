using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using PlaywrightAutomationDemo.Tests;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Tests;

[TestFixture]
public class FileDownloadTests : PageTest
{
    private FileDownloadPage _fileDownloadPage = null!;

    [SetUp]
    public async Task SetUp()
    {
        _fileDownloadPage = new FileDownloadPage(Page);
        await _fileDownloadPage.NavigateAsync();
    }

    [Test]
    public async Task NavigateAsync_OnPageLoad_DownloadPageTitleIsVisible()
    {
        await Expect(Page).ToHaveTitleAsync(new Regex("The Internet"));
        Console.WriteLine("PASS: FileDownloadTests - NavigateAsync_OnPageLoad_DownloadPageTitleIsVisible - Page title matches 'The Internet' on initial load");
    }

    [Test]
    public async Task NavigateAsync_OnPageLoad_SampleUploadTxtLinkIsVisible()
    {
        var sampleUploadTxtLink = Page.GetByRole(AriaRole.Link, new() { Name = "sample_upload.txt" });
        await Expect(sampleUploadTxtLink).ToBeVisibleAsync();
        Console.WriteLine("PASS: FileDownloadTests - NavigateAsync_OnPageLoad_SampleUploadTxtLinkIsVisible - sample_upload.txt link is visible on initial page load");
    }

    [Test]
    public async Task NavigateAsync_OnPageLoad_CorrectUrlIsLoaded()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*/download$"));
        Console.WriteLine("PASS: FileDownloadTests - NavigateAsync_OnPageLoad_CorrectUrlIsLoaded - Page URL ends with /download as expected");
    }

    [Test]
    public async Task ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadPathIsNotEmpty()
    {
        var downloadedFilePath = await _fileDownloadPage.ClickSampleUploadTxtAndVerifyDownloadAsync();
        Assert.That(downloadedFilePath, Is.Not.Empty, "Expected a non-empty file path after download completed");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadPathIsNotEmpty - Download path returned is not empty");
    }

    [Test]
    public async Task ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists()
    {
        var downloadedFilePath = await _fileDownloadPage.ClickSampleUploadTxtAndVerifyDownloadAsync();
        Assert.That(System.IO.File.Exists(downloadedFilePath), Is.True, $"Expected downloaded file to exist at path: {downloadedFilePath}");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists - Downloaded file exists on disk at path: {downloadedFilePath}");
    }
}