namespace PlaywrightAutomationDemo.Config;

public static class TestConfig
{
    public const string BaseUrl = "https://jsonplaceholder.typicode.com";
    public const string UiBaseUrl = "https://the-internet.herokuapp.com";
    public const int DefaultWaitMs = 10000;
    
    // Walks up from bin\Debug\net10.0 until it finds the folder with the .csproj
    public static readonly string ProjectRoot = FindProjectRoot();
    public static readonly string DbPath = Path.Combine(ProjectRoot, "TestData", "testdb.sqlite");

    private static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !dir.GetFiles("*.csproj").Any())
            dir = dir.Parent;
        return dir?.FullName ?? AppContext.BaseDirectory;
    }

}