using System.Collections.ObjectModel;
using System.Windows.Input;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.ViewModels;

public class BoardGamesViewModel : ObservableObject
{
    private readonly IAppStorageService _appStorageService;
    private readonly ILogger<BoardGamesViewModel> _logger;
    private readonly IAppNavigationService _navigationService;
    private GameViewModel? _selectedGame;

    public BoardGamesViewModel(IAppStorageService appStorageService, ILogger<BoardGamesViewModel> logger,
        IAppNavigationService navigationService)
    {
        _appStorageService = appStorageService;
        _logger = logger;
        _navigationService = navigationService;
        BoardGames = [];
        AddGameCommand = new AsyncRelayCommand(AddBoardGame);
        EditGameCommand = new AsyncRelayCommand(EditBoardGame);
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
    public IAsyncRelayCommand AddGameCommand { get; private set; }
    public IAsyncRelayCommand EditGameCommand { get; private set; }

    private async Task LoadBoardGames()
    {
        var games = await _appStorageService.SetupLocalStorage();
        foreach (var game in games) BoardGames.Add(new GameViewModel(game));
    }

    private async Task AddBoardGame()
    {
        await _navigationService.NavigateTo("SaveBoardGamePage", false);
    }

    private async Task EditBoardGame()
    {
        await _navigationService.NavigateTo("SaveBoardGamePage", true);
    }

    public async Task SaveBoardGames()
    {
        var games = new List<BoardGame>();
        foreach (var gameViewModel in BoardGames)
            games.Add(new BoardGame(gameViewModel.Name, gameViewModel.Description, gameViewModel.ImageUri,
                gameViewModel.MinimumNumberOfPlayers, gameViewModel.MaximumNumberOfPlayers,
                gameViewModel.MinutesPerGame));

        await _appStorageService.SaveLocalData(games);
    }

    private void DeleteBoardGame(GameViewModel boardGame)
    {
        // Should remove the game from the list of games
    }
}