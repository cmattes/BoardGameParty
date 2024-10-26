using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace UITests;

public abstract class BaseTest
{
    private readonly AppiumSetup _setup;
    protected AppiumDriver App => _setup.App;

    public BaseTest(AppiumSetup setup)
    {
        _setup = setup;
    }
    
    protected AppiumElement FindUIElement(string id)
    {
        if (App is WindowsDriver)
        {
            return App.FindElement(MobileBy.AccessibilityId(id));
        }

        return App.FindElement(MobileBy.Id(id));
    }

}