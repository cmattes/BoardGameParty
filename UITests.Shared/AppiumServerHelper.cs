using OpenQA.Selenium.Appium.Service;
//using OpenQA.Selenium.Appium.Service.Options;

namespace UITests;

public static class AppiumServerHelper
{
    private static AppiumLocalService? _appiumLocalService;

    public const string DefaultHostAddress = "127.0.0.1";
    public const int DefaultHostPort = 4723;

    public static void StartAppiumLocalServer(string host = DefaultHostAddress, int port = DefaultHostPort)
    {
        if (_appiumLocalService is not null)
        {
            return;
        }

        // Use if mobile: shell is needed for testing the UI
        //var args = new OptionCollector().AddArguments(new KeyValuePair<string, string>("--allow-insecure=adb_shell", ""));
        
        // Example that would get added to BaseTest: 
        // var folderExists = App.ExecuteScript("mobile: shell", new Dictionary<string, object>
        // {
        //     { "command", "run-as" },
        //      { "args", new[] { "com.mattesgames.boardgameparty", "cat", $"{localBoardGamesData}" } }
        // });

        var builder = new AppiumServiceBuilder()
            .WithIPAddress(host)
            .UsingPort(port);
            //.WithArguments(args);

        // Start the server with the builder
        _appiumLocalService = builder.Build();
        _appiumLocalService.Start();
    }

    public static void DisposeAppiumLocalServer()
    {
        _appiumLocalService?.Dispose();
    }
}