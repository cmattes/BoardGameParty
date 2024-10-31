namespace BoardGameParty;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        BoardGameCollectionView.ItemsSource = GetBoardGames();
        //BoardGameCollectionView.ItemsSource = null;
    }

    private List<BoardGame> GetBoardGames()
    {
        return new List<BoardGame>
        {
            new()
            {
                GameName = "TestGame1", GameDescription = "A game that is for testing", MinutesPerGame = 30,
                NumberOfPlayers = (1, 4), GameImageURI = "dotnet_bot.png"
            },
            new()
            {
                GameName = "TestGame2", GameDescription = "A game that is for testing", MinutesPerGame = 20,
                NumberOfPlayers = (3, 5), GameImageURI = "dotnet_bot.png"
            }
        };
    }
}