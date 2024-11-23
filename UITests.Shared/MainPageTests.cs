using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Xunit.Abstractions;

namespace UITests;

[Collection("UICollection")]
public class MainPageTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    public MainPageTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
        
        VerifyAppIsReady("Board Game Party");
    }
    
    [Fact]
    public void Verify_app_launches()
    {
        // Arrange
        var screenshotName = "app_screenshot.png";

        // Act
        App.GetScreenshot().SaveAsFile(screenshotName);
        Thread.Sleep(500);

        // Assert
        Assert.True(File.Exists(screenshotName), "Failed to gather a screenshot.");
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
        
        Assert.True(VerifyAppIsReady("Board Game Party"));
    }
}