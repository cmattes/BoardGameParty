using BoardGameParty.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.Models;

public class MainPageViewModel
{
    private readonly IAppStorage _appStorage;
    private readonly ILogger<MainPageViewModel> _logger;
    private string _storageDirectory;

    public MainPageViewModel(IAppStorage appStorage, ILogger<MainPageViewModel> logger)
    {
        _appStorage = appStorage;
        _logger = logger;
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