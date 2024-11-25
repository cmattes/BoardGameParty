using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using Xunit.Abstractions;

namespace UITests;

[Collection("UICollection")]
public class MainPageTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    public MainPageTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Default_message_displays_if_no_games_exist_in_list()
    {
        try
        {
            SetupTestData(false);
            RestartApp();
            var expectedText = "No games to display yet!";

            var emptycv = FindUIElement("EmptyBoardGamecv");
            var emptycvText = emptycv.Text;
            
            Assert.Equal(expectedText, emptycvText);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }

    [Fact]
    public void App_displays_list_of_games()
    {
        try
        {
            SetupTestData(true);
            RestartApp();
            var expectedGameTitle = "TestGame1";
            
            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var firstGameTitle = FindUIElement("Name", games[0]);
            Assert.Equal(expectedGameTitle, firstGameTitle.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
    
    [Fact]
    public void Default_message_displays_when_no_game_is_selected()
    {
        try
        {
            SetupTestData(true);
            RestartApp();
            var expectedText = "Select a game to see it's details";
            
            var emptyDetails = FindUIElement("EmptyDetailsLabel");
            var emptyDetailsText = emptyDetails.Text;
            
            Assert.Equal(expectedText, emptyDetailsText);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }

    [Fact]
    public void Game_details_displayed_when_game_is_selected()
    {
        try
        {
            SetupTestData(true);
            RestartApp();
            var expectedGameTitle = "TestGame1";
            
            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);
            var firstGameTitle = FindUIElement("Name", games[0]);
            
            games[0].Click();
            
            var gameDetails = FindUIElement("GameDetails");
            var gameDetailsGameName = FindUIElement("GameDetailsGameName", gameDetails);
            Assert.Equal(expectedGameTitle, gameDetailsGameName.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
    
    [Fact]
    public async Task Navigate_to_non_existing_page_does_not_work()
    {
        SetupTestData(false);
        RestartApp();

        await NavigationService.NavigateTo("UnknownPage", false);
        
        Assert.True(VerifyAppIsReady(_boardGamesPageTitle));
    }
    
    [Fact]
    public void Add_button_navigates_to_save_page()
    {
        try
        {
            SetupTestData(false);
            RestartApp();
            
            var addButton = FindUIElementByAccessibilityId("AddBoardGameButton");
            addButton.Click();
            
            Assert.True(VerifyAppIsReady(_savePageTitle));
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
    
    [Fact]
    public void When_no_change_on_save_page_cancel_button_returns_to_main_page()
    {
        try
        {
            SetupTestData(false);
            RestartApp();
            
            var addButton = FindUIElementByAccessibilityId("AddBoardGameButton");
            addButton.Click();
            Assert.True(VerifyAppIsReady(_savePageTitle));
            
            var cancelButton = FindUIElement("CancelSaveButton");
            cancelButton.Click();
            
            Assert.True(VerifyAppIsReady(_boardGamesPageTitle));
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
    
    [Fact]
    public void When_change_occurs_on_save_page_cancel_button_displays_alert()
    {
        try
        {
            SetupTestData(false);
            RestartApp();
            var expectedTitle = "Cancel Save";
            
            var addButton = FindUIElementByAccessibilityId("AddBoardGameButton");
            addButton.Click();
            Assert.True(VerifyAppIsReady(_savePageTitle));
            
            var nameTextField = FindUIElement("NameTextField");
            nameTextField.SendKeys("Test");
            SendReturnKey(nameTextField);
            
            var cancelButton = FindUIElement("CancelSaveButton");
            cancelButton.Click();
            
            if (App is IOSDriver)
            {
                Assert.True(VerifyAlertIsDisplayed("Cancel Save"));
            }
            else if (App is AndroidDriver)
            {
                Assert.True(VerifyAlertIsDisplayed("com.mattesgames.boardgameparty:id/alertTitle"));
            }
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
    
    [Fact]
    public void When_name_updates_on_save_page_save_button_enables()
    {
        try
        {
            SetupTestData(false);
            RestartApp();

            var addButton = FindUIElementByAccessibilityId("AddBoardGameButton");
            addButton.Click();
            Assert.True(VerifyAppIsReady(_savePageTitle));
            
            var saveButton = FindUIElement("SaveButton");
            Assert.False(saveButton.Enabled);
            
            var nameTextField = FindUIElement("NameTextField");
            nameTextField.SendKeys("Test");
            SendReturnKey(nameTextField);
            
            Assert.True(saveButton.Enabled);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }

    [Fact]
    public void Save_page_save_button_returns_and_adds_a_new_game_to_the_main_page()
    {
        try
        {
            SetupTestData(false);
            RestartApp();

            var expectedText = "No games to display yet!";
            var emptycv = FindUIElement("EmptyBoardGamecv");
            Assert.Equal(expectedText, emptycv.Text);

            var expectedGameTitle = "TestGame1";
            var addButton = FindUIElementByAccessibilityId("AddBoardGameButton");
            addButton.Click();
            Assert.True(VerifyAppIsReady(_savePageTitle));
            
            var nameTextField = FindUIElement("NameTextField");
            nameTextField.SendKeys("TestGame1");
            SendReturnKey(nameTextField);
            
            var saveButton = FindUIElement("SaveButton");
            saveButton.Click();
            Assert.True(VerifyAppIsReady(_boardGamesPageTitle));

            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var firstGameTitle = FindUIElement("Name", games[0]);
            Assert.Equal(expectedGameTitle, firstGameTitle.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
}