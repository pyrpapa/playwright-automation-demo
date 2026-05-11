using Microsoft.Playwright;
using PlaywrightAutomationDemo.Config;

namespace PlaywrightAutomationDemo.Pages;

public class FileUploadPage
{
    private readonly IPage _page;

    // Locators
    private ILocator FileUploaderHeading =>   _page.GetByRole(AriaRole.Heading, new() { Name = "File Uploader" });
    private ILocator UploadButton => _page.GetByRole(AriaRole.Button, new() { Name = "Upload" });
    private ILocator FileInput => _page.GetByRole(AriaRole.Button, new() { Name = "Choose File" });
    private ILocator FileUploadedHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "File Uploaded!" });
    private ILocator UploadedFileName => _page.GetByText("test.txt");
    private ILocator InternalServerError => _page.GetByRole(AriaRole.Heading, new() { Name = "Internal Server Error" });

    public FileUploadPage(IPage page)
    {
        _page = page;
    }

    public async Task<bool>UploadFileSuccessAsync()
    {
        await _page.GotoAsync(TestConfig.UiBaseUrl + "/upload");
        await FileInput.ClickAsync();
        await _page.SetInputFilesAsync("input[type='file']", "C:\\Projects\\playwright-automation-demo\\Files\\test.txt");
        // Wait for 2 seconds
        await _page.WaitForTimeoutAsync(2000);
        await UploadButton.ClickAsync();
        return await FileUploadedHeading.IsVisibleAsync();
    }

    public async Task<bool>UploadFileFailureAsync()
    {
        await _page.GotoAsync(TestConfig.UiBaseUrl + "/upload");
        await FileInput.ClickAsync();
        try
        {
            await _page.SetInputFilesAsync("input[type='file']", "C:\\Projects\\playwright-automation-demo\\Files\\notexist.txt");
            return false; // if we get here, no exception was thrown
        }
        catch (FileNotFoundException)
        {
            return true; // expected - file doesn't exist
        }
    }

    
}