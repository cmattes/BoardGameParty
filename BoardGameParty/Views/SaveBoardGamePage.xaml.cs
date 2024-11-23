using BoardGameParty.Models;
using BoardGameParty.ViewModels;

namespace BoardGameParty.Views;

public partial class SaveBoardGamePage : ContentPage
{
    public SaveBoardGamePage(bool updatingGame)
    {
        var saveBoardGameViewModel = new SaveBoardGameViewModel(updatingGame)
        {
            ChangingGame = updatingGame 
                ? App.CloneGameViewModel(App.BoardGamesListViewModel.SelectedGame) 
                : new GameViewModel()
        };

        BindingContext = saveBoardGameViewModel;
        
        InitializeComponent();
        this.Loaded += SaveBoardGamePage_Loaded;
    }
    
    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is SaveBoardGameViewModel saveGameViewModel && saveGameViewModel.CancelCommand.CanExecute(null))
            saveGameViewModel.CancelCommand.Execute(null);

        return true;
    }

    private void SaveBoardGamePage_Loaded(object? sender, EventArgs e)
    {
        EnableSwipeGesture(false);
    }
    
    private void EnableSwipeGesture(bool enable)
    {
#if IOS
        if (Handler is not IPlatformViewHandler platformViewHandler) return;
        
        if (platformViewHandler.ViewController?.NavigationController != null)
        {
            platformViewHandler.ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = enable;
        }
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BindingContext = null;
        this.Loaded -= SaveBoardGamePage_Loaded;
        EnableSwipeGesture(true);
    }
}