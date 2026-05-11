using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightAutomationDemo.Pages;

namespace PlaywrightAutomationDemo.Tests.UI;

[TestFixture]
public class FileUpload : PageTest
{
    private FileUploadPage _fileuploadPage;

    [SetUp]
    public void SetUp()
    {
        _fileuploadPage = new FileUploadPage(Page);
    }

    [Test]
    public async Task FileUploadSuccess()
    {
        Assert.That(await _fileuploadPage.UploadFileSuccessAsync(), Is.True);
        Console.WriteLine($"File uploaded successfully.");
    }

    [Test]
    public async Task FileUploadFailure()
    {
        Assert.That(await _fileuploadPage.UploadFileFailureAsync(), Is.True);
        Console.WriteLine($"Unable to upload file due to Internal Server Error.");

    }
}