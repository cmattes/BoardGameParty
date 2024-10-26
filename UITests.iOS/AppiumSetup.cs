using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace UITests;

public class AppiumSetup : IDisposable
{
    private AppiumDriver? driver;
    
    public AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");
    
    public AppiumSetup()
    {
        // If you started an Appium server manually, make sure to comment out the next line
        // This line starts a local Appium server for you as part of the test run
        AppiumServerHelper.StartAppiumLocalServer();

        var iOSOptions = new AppiumOptions
        {
            // Specify XCUITest as the driver, typically don't need to change this
            AutomationName = "XCUITest",
            // Always iOS for iOS
            PlatformName = "iOS",
            // iOS Version
            PlatformVersion = "18.0",
            // Don't specify if you don't want a specific device
            DeviceName = "iPhone 16 Pro Max",
            // The full path to the .app file to test or the bundle id if the app is already installed on the device
            App = "com.mattesgames.boardgameparty",
        };

        // Note there are many more options that you can use to influence the app under test according to your needs

        driver = new IOSDriver(iOSOptions);
    }

    public void Dispose()
    {
        driver?.Quit();

        // If an Appium server was started locally above, make sure we clean it up here
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}

[CollectionDefinition("UICollection")]
public class DatabaseCollection : ICollectionFixture<AppiumSetup>;