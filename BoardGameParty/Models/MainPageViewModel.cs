using BoardGameParty.Interfaces;

namespace BoardGameParty.Models;

public class MainPageViewModel
{
    private readonly IAppStorage _appStorage;
    private string _storageDirectory;

    // public MainPageViewModel(IAppStorage appStorage)
    // {
    //     _appStorage = appStorage;
    //     //_appStorage.SetupLocalStorage();
    //
    // }
    
    public List<BoardGame> GetBoardGames()
    {
        return new List<BoardGame>
        {
            new("TestGame1")
            {
                Description = "A game that is for testing", MinutesPerGame = 30,
                MinimumNumberOfPlayers = 1, MaximumNumberOfPlayers = 4, ImageURI = "dotnet_bot.png"
            },
            new("TestGame2")
            {
                Description = "A game that is for testing", MinutesPerGame = 20,
                MinimumNumberOfPlayers = 3, MaximumNumberOfPlayers = 5, ImageURI = "dotnet_bot.png"
            }
        };
    }
}