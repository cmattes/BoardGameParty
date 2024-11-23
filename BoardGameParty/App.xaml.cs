using System.Text.Json;
using BoardGameParty.ViewModels;

namespace BoardGameParty;

public partial class App : Application
{
    public App(BoardGamesViewModel boardGamesListViewModel)
    {
        InitializeComponent();

        MainPage = new AppShell();

        BoardGamesListViewModel = boardGamesListViewModel;
    }

    public static BoardGamesViewModel BoardGamesListViewModel { get; private set; }

    public static GameViewModel? CloneGameViewModel(GameViewModel? source)
    {
        return source is null ? default : JsonSerializer.Deserialize<GameViewModel>(JsonSerializer.Serialize(source));
    }
}