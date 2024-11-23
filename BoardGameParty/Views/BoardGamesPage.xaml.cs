namespace BoardGameParty.Views;

public partial class BoardGamesPage : ContentPage
{
    public BoardGamesPage()
    {
        InitializeComponent();
        
        BtnAdd.Clicked += async (s, e) => await Navigation.PushAsync(new SaveBoardGamePage(false));
        //BtnUpdate.Clicked += async (s, e) => await Navigation.PushAsync(new SaveBoardGamePage(true));
    }
}