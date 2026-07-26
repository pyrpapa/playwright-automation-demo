using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaywrightAutomationDemo.Tests;

[TestFixture]
public class FileDownloadTests : PageTest
{
    private FileDownloadPage _fileDownloadPage = null!;

    // Reuses the same file FileUploadTests already ships with the repo (Files/test.txt),
    // copied next to the test binaries at build time (see the .csproj).
    private const string TestFileName = "test.txt";

    [SetUp]
    public async Task SetUp()
    {
        _fileDownloadPage = new FileDownloadPage(Page);

        // Upload the file ourselves first so the download page is guaranteed to have
        // something to show - see the comment on FileDownloadPage.UploadFileAsync for why
        // we can't just assume a previously-uploaded file is still sitting on the server.
        var uploadFilePath = Path.Combine(AppContext.BaseDirectory, "Files", TestFileName);
        await _fileDownloadPage.UploadFileAsync(uploadFilePath);

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
        Assert.That(await _fileDownloadPage.IsFileLinkVisibleAsync(TestFileName), Is.True,
            $"Expected a link to '{TestFileName}' to be visible on the download page after uploading it");
        Console.WriteLine($"PASS: FileDownloadTests - NavigateAsync_OnPageLoad_SampleUploadTxtLinkIsVisible - {TestFileName} link is visible on initial page load");
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
        var downloadedFilePath = await _fileDownloadPage.ClickFileAndVerifyDownloadAsync(TestFileName);
        Assert.That(downloadedFilePath, Is.Not.Empty, "Expected a non-empty file path after download completed");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadPathIsNotEmpty - Download path returned is not empty");
    }

    [Test]
    public async Task ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists()
    {
        var downloadedFilePath = await _fileDownloadPage.ClickFileAndVerifyDownloadAsync(TestFileName);
        Assert.That(System.IO.File.Exists(downloadedFilePath), Is.True, $"Expected downloaded file to exist at path: {downloadedFilePath}");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists - Downloaded file exists on disk at path: {downloadedFilePath}");
    }
}
