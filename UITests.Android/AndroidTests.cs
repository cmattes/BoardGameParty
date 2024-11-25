using Xunit.Abstractions;

namespace UITests;

[Collection("UICollection")]
public class AndroidTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AndroidTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Edit_button_updates_the_selected_game()
    {
        try
        {
            SetupTestData(true);
            RestartApp();

            var expectedMinutesPerGame = "30";
            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var currentMinutesPerGame = FindUIElement("MinutesPerGame", games[0]);
            Assert.Equal(expectedMinutesPerGame, currentMinutesPerGame.Text);
            games[0].Click();

            var gameDetails = FindUIElement("GameDetails");
            var currentMinutesPerGameDetails = FindUIElement("MinutesPerGameDetails", gameDetails);
            Assert.Equal(expectedMinutesPerGame, currentMinutesPerGameDetails.Text);

            var editButton = FindUIElement("EditBoardGameButton");
            editButton.Click();

            var newExpectedMinutesPerGame = "15";
            gameList = FindUIElement("BoardGamecv");
            games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var newMinutesPerGame = FindUIElement("MinutesPerGame", games[0]);
            Assert.Equal(newExpectedMinutesPerGame, newMinutesPerGame.Text);
            games[0].Click();

            gameDetails = FindUIElement("GameDetails");
            var newMinutesPerGameDetails = FindUIElement("MinutesPerGameDetails", gameDetails);
            Assert.Equal(newExpectedMinutesPerGame, newMinutesPerGameDetails.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }

    [Fact]
    public void Swiping_a_game_reveals_the_delete_button()
    {
        try
        {
            SetupTestData(true);
            RestartApp();

            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var expectedGameName = "TestGame1";
            var firstGameName = FindUIElement("Name", games[0]);
            Assert.Equal(expectedGameName, firstGameName.Text);
            App.ExecuteScript("mobile: swipeGesture", new Dictionary<string, object>
            {
                { "elementId", games[0].Id },
                { "direction", "left" },
                { "percent", 1.0 }
            });

            var expectButtonText = "Delete";
            var deleteButton = FindUIElement("DeleteBoardGameButton");
            Assert.Equal(expectButtonText, deleteButton.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }

    [Fact]
    public void Delete_button_deletes_the_swiped_game()
    {
        try
        {
            SetupTestData(true);
            RestartApp();

            var gameList = FindUIElement("BoardGamecv");
            var games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var expectedGameName = "TestGame1";
            var firstGameName = FindUIElement("Name", games[0]);
            Assert.Equal(expectedGameName, firstGameName.Text);
            App.ExecuteScript("mobile: swipeGesture", new Dictionary<string, object>
            {
                { "elementId", games[0].Id },
                { "direction", "left" },
                { "percent", 1.0 }
            });

            var deleteButton = FindUIElement("DeleteBoardGameButton");
            deleteButton.Click();

            gameList = FindUIElement("BoardGamecv");
            games = FindUIElements("Game", gameList);
            Assert.NotNull(games);
            Assert.True(games.Count > 0);

            var newExpectedGameName = "TestGame2";
            firstGameName = FindUIElement("Name", games[0]);
            Assert.Equal(newExpectedGameName, firstGameName.Text);
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            Assert.Fail();
        }
    }
}