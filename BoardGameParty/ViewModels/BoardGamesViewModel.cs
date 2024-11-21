using System.Collections.ObjectModel;
using System.Windows.Input;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.ViewModels;

public class BoardGamesViewModel : ObservableObject
{
    private readonly IAppStorage _appStorage;
    private readonly ILogger<BoardGamesViewModel> _logger;
    private GameViewModel? _selectedGame;

    public BoardGamesViewModel(IAppStorage appStorage, ILogger<BoardGamesViewModel> logger)
    {
        BoardGames = [];
        _appStorage = appStorage;
        _logger = logger;
        Task.Run(LoadBoardGames);
    }

    public ObservableCollection<GameViewModel> BoardGames { get; set; }

    public GameViewModel? SelectedGame
    {
        get => _selectedGame;
        set => SetProperty(ref _selectedGame, value);
    }

    public ICommand AddGameCommand { get; private set; }

    private async Task LoadBoardGames()
    {
        var games = await _appStorage.SetupLocalStorage();
        foreach (var game in games)
        {
            BoardGames.Add(new GameViewModel(game));
        }
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