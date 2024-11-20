using System.Collections.ObjectModel;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.ViewModels;

public class BoardGamesViewModel : ObservableObject
{
    private readonly IAppStorage _appStorage;
    private readonly ILogger<BoardGamesViewModel> _logger;
    private bool _gamesUpdating;

    private GameViewModel? _selectedGame;

    public BoardGamesViewModel(IAppStorage appStorage, ILogger<BoardGamesViewModel> logger)
    {
        BoardGames = [];
        _appStorage = appStorage;
        _logger = logger;
        GamesUpdating = true;
        Task.Run(LoadBoardGames);
    }

    public ObservableCollection<GameViewModel> BoardGames { get; set; }

    public GameViewModel? SelectedGame
    {
        get => _selectedGame;
        set => SetProperty(ref _selectedGame, value);
    }

    public bool GamesUpdating
    {
        get => _gamesUpdating;
        set => SetProperty(ref _gamesUpdating, value);
    }

    private async Task LoadBoardGames()
    {
        var games = await _appStorage.SetupLocalStorage();
        foreach (var game in games) BoardGames.Add(new GameViewModel(game));
        GamesUpdating = false;
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