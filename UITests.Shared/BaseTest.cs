using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace UITests;

public abstract class BaseTest
{
    private readonly AppiumSetup _setup;
    protected AppiumDriver App => _setup.App;

    public BaseTest(AppiumSetup setup)
    {
        _setup = setup;
        _wait = new WebDriverWait(App, TimeSpan.FromSeconds(30))
        {
            PollingInterval = TimeSpan.FromMilliseconds(1000),
        };
        _wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));
    }
    
    private readonly WebDriverWait _wait;
    protected bool VerifyAppIsReady(string title)
    {
        if (App is IOSDriver)
        {
            return _wait.Until(t => App.FindElement(MobileBy.XPath("//XCUIElementTypeStaticText[@name=\"Board Game Party\"]")).Displayed);
        }
        else if (App is AndroidDriver)
        {
            return _wait.Until(t => App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Board Game Party\"]")).Displayed);
        }

        return true;
    }
    
    protected AppiumElement FindUIElement(string id)
    {
        // Add if needed for Windows
        // if (App is WindowsDriver)
        // {
        //     return App.FindElement(MobileBy.AccessibilityId(id));
        // }

        return App.FindElement(MobileBy.Id(id));
    }
    
    protected AppiumElement FindUIElement(string id, AppiumElement parentElement)
    {
        return parentElement.FindElement(MobileBy.Id(id));
    }
    
    protected  ReadOnlyCollection<AppiumElement>? FindUIElements(string id)
    {
        return App.FindElements(MobileBy.Id(id));
    }
    
    protected  ReadOnlyCollection<AppiumElement>? FindUIElements(string id, AppiumElement parentElement)
    {
        return parentElement.FindElements(MobileBy.Id(id));
    }

}