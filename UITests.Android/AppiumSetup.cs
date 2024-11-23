using BoardGameParty.Interfaces;
using BoardGameParty.Services;
using Microsoft.Extensions.Logging.Abstractions;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace UITests;

public class AppiumSetup : IDisposable
{
    private readonly AppiumDriver? driver;
    private readonly IAppNavigationService navService;

    public AppiumSetup()
    {
        navService = new AppNavigationService(new NullLogger<AppNavigationService>(), new AppAlertService());
        // If you started an Appium server manually, make sure to comment out the next line
        // This line starts a local Appium server for you as part of the test run
        AppiumServerHelper.StartAppiumLocalServer();

        var androidOptions = new AppiumOptions
        {
            // Specify UIAutomator2 as the driver, typically don't need to change this
            AutomationName = "UIAutomator2",
            // Always Android for Android
            PlatformName = "Android"
        };

        androidOptions.AddAdditionalAppiumOption("avd", "Pixel_3a_API_34");
        androidOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, "true");
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage,
            "com.mattesgames.boardgameparty");
        androidOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity,
            "com.mattesgames.boardgameparty.MainActivity");

        // Note there are many more options that you can use to influence the app under test according to your needs

        driver = new AndroidDriver(androidOptions);
    }

    public AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    public IAppNavigationService NavigationService =>
        navService ?? throw new NullReferenceException("NavService is null");

    public void Dispose()
    {
        driver?.TerminateApp("com.mattesgames.boardgameparty");

        driver?.Quit();

        // If an Appium server was started locally above, make sure we clean it up here
        AppiumServerHelper.DisposeAppiumLocalServer();
    }
}

[CollectionDefinition("UICollection")]
public class DatabaseCollection : ICollectionFixture<AppiumSetup>;