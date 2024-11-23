using System.Collections.ObjectModel;
using System.Windows.Input;
using BoardGameParty.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.ViewModels;

public class BoardGamesViewModel : ObservableObject
{
    private readonly IAppStorageService _appStorageService;
    private readonly ILogger<BoardGamesViewModel> _logger;
    private GameViewModel? _selectedGame;

    public BoardGamesViewModel(IAppStorageService appStorageService, ILogger<BoardGamesViewModel> logger)
    {
        _appStorageService = appStorageService;
        _logger = logger;
        BoardGames = [];
        DeleteGameCommand = new Command<GameViewModel>(DeleteBoardGame);
        Task.Run(LoadBoardGames);
    }

    public ObservableCollection<GameViewModel> BoardGames { get; set; }

    public GameViewModel? SelectedGame
    {
        get => _selectedGame;
        set => SetProperty(ref _selectedGame, value);
    }

    public ICommand DeleteGameCommand { get; private set; }

    private async Task LoadBoardGames()
    {
        var games = await _appStorageService.SetupLocalStorage();
        foreach (var game in games) BoardGames.Add(new GameViewModel(game));
    }

    private void DeleteBoardGame(GameViewModel boardGame)
    {
        // Should remove the game from the list of games
    }
}