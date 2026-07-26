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
    private string _uploadedFileName = null!;
    private string _tempUploadFilePath = null!;

    [SetUp]
    public async Task SetUp()
    {
        _fileDownloadPage = new FileDownloadPage(Page);

        // Give the uploaded file a unique name every run (a copy of the repo's
        // Files/test.txt, renamed) instead of a fixed name like "test.txt". The
        // /download page on this shared, public server can already have files sitting
        // on it - other test runs, leftovers from previous CI runs, etc. - so a fixed
        // name risks colliding with something we didn't upload and don't control. A
        // GUID-suffixed name guarantees we're only ever looking at our own file.
        _uploadedFileName = $"playwright-download-{Guid.NewGuid():N}.txt";
        var sourceFilePath = Path.Combine(AppContext.BaseDirectory, "Files", "test.txt");
        _tempUploadFilePath = Path.Combine(Path.GetTempPath(), _uploadedFileName);
        File.Copy(sourceFilePath, _tempUploadFilePath, overwrite: true);

        // Upload the file ourselves first so the download page is guaranteed to have
        // something to show - see the comment on FileDownloadPage.UploadFileAsync for why
        // we can't just assume a previously-uploaded file is still sitting on the server.
        await _fileDownloadPage.UploadFileAsync(_tempUploadFilePath);

        await _fileDownloadPage.NavigateAsync();
    }

    [TearDown]
    public void TearDown()
    {
        if (_tempUploadFilePath != null && File.Exists(_tempUploadFilePath))
        {
            File.Delete(_tempUploadFilePath);
        }
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
        Assert.That(await _fileDownloadPage.IsFileLinkVisibleAsync(_uploadedFileName), Is.True,
            $"Expected a link to '{_uploadedFileName}' to be visible on the download page after uploading it");
        Console.WriteLine($"PASS: FileDownloadTests - NavigateAsync_OnPageLoad_SampleUploadTxtLinkIsVisible - {_uploadedFileName} link is visible on initial page load");
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
        var downloadedFilePath = await _fileDownloadPage.ClickFileAndVerifyDownloadAsync(_uploadedFileName);
        Assert.That(downloadedFilePath, Is.Not.Empty, "Expected a non-empty file path after download completed");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadPathIsNotEmpty - Download path returned is not empty");
    }

    [Test]
    public async Task ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists()
    {
        var downloadedFilePath = await _fileDownloadPage.ClickFileAndVerifyDownloadAsync(_uploadedFileName);
        Assert.That(System.IO.File.Exists(downloadedFilePath), Is.True, $"Expected downloaded file to exist at path: {downloadedFilePath}");
        Console.WriteLine("PASS: FileDownloadTests - ClickSampleUploadTxtAndVerifyDownloadAsync_ValidFileLink_DownloadedFileExists - Downloaded file exists on disk at path: {downloadedFilePath}");
    }
}
