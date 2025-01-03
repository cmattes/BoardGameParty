using BoardGameParty.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
//using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace UITests;

public abstract class BaseTest
{
    private readonly AppiumSetup _setup;
    protected AppiumDriver App => _setup.App;
    protected IAppNavigationService NavigationService => _setup.NavigationService;
    protected string _boardGamesPageTitle = "Board Game Party";
    protected string _savePageTitle = "Save Game";

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
            return _wait.Until(t => App.FindElement(MobileBy.XPath($"//XCUIElementTypeStaticText[@name=\"{title}\"]")).Displayed);
        }
        else if (App is AndroidDriver)
        {
            return _wait.Until(t => App.FindElement(MobileBy.XPath($"//android.widget.TextView[@text=\"{title}\"]")).Displayed);
        }

        return false;
    }
    
    protected bool VerifyAlertIsDisplayed(string title)
    {
        if (App is IOSDriver)
        {
            
            return _wait.Until(t => App.FindElement(MobileBy.XPath($"//XCUIElementTypeStaticText[@name=\"{title}\"]")).Displayed);
        }
        else if (App is AndroidDriver)
        {
            
            return _wait.Until(t => App.FindElement(MobileBy.XPath($"//android.widget.TextView[@resource-id=\"{title}\"]")).Displayed);
        }

        return false;
    }

    protected void RestartApp()
    {
        App.TerminateApp("com.mattesgames.boardgameparty");
        Thread.Sleep(1000);
        App.ActivateApp("com.mattesgames.boardgameparty");
        Thread.Sleep(1000);
        VerifyAppIsReady(_boardGamesPageTitle);
    }
    
    protected void SetupTestData(bool testNeedsTestData)
    {
        var localBoardGamesData = "@com.mattesgames.boardgameparty/files/BoardGameData/LocalBoardGames.json";
        
        if (App is IOSDriver)
        {
            localBoardGamesData = "@com.mattesgames.boardgameparty:data/Library/BoardGameData/LocalBoardGames.json";
        }

        try
        {
            App.ExecuteScript("mobile: deleteFile", new Dictionary<string, object>()
            {
                {"remotePath", $"{localBoardGamesData}"}
            });
        }
        catch (Exception e)
        {
            // ignored
        }
        
        if (testNeedsTestData)
        {
            var contents = File.ReadAllText("BoardGamesTestData.json");
            App.PushFile($"{localBoardGamesData}", contents);
        }
    }

    protected void SendReturnKey(AppiumElement nameTextField)
    {
        if (App is IOSDriver)
        {
            Thread.Sleep(1000);
            nameTextField.SendKeys(Keys.Return);
        }
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
    
    protected AppiumElement FindUIElementByAccessibilityId(string id)
    { 
        return App.FindElement(MobileBy.AccessibilityId(id));
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