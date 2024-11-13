using BoardGameParty.Interfaces;

namespace BoardGameParty.Models;

public class MainPageViewModel
{
    private readonly IAppStorage _appStorage;
    private string _storageDirectory;

    public MainPageViewModel(IAppStorage appStorage)
    {
        _appStorage = appStorage;
        Task.Run(LoadBoardGames).Wait();
    }

    public IList<BoardGame> BoardGames { get; set; }

    private async Task LoadBoardGames()
    {
        BoardGames = await _appStorage.SetupLocalStorage();
    }

    public async Task AddNewBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }

    public async Task DeleteBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }

    public async Task UpdateBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }
}