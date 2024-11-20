using BoardGameParty.ViewModels;

namespace BoardGameParty;

public partial class App : Application
{
    public static BoardGamesViewModel BoardGamesListViewModel { get; private set; }
    
    public App( BoardGamesViewModel boardGamesListViewModel )
    {
        InitializeComponent();

        MainPage = new AppShell();
        
        BoardGamesListViewModel = boardGamesListViewModel;
    }
}