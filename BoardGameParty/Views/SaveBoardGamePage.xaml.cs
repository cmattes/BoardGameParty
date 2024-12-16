using BoardGameParty.Interfaces;
using BoardGameParty.ViewModels;

namespace BoardGameParty.Views;

public partial class SaveBoardGamePage : ContentPage
{
    public SaveBoardGamePage(bool updatingGame, IAppNavigationService navigationService, IAppAlertService alertService)
    {
        var saveBoardGameViewModel = new SaveBoardGameViewModel(updatingGame, navigationService, alertService)
        {
            ChangingGame = updatingGame 
                ? App.CloneGameViewModel(App.BoardGamesListViewModel.SelectedGame) 
                : new GameViewModel()
        };

        BindingContext = saveBoardGameViewModel;
        
        InitializeComponent();
        this.Loaded += SaveBoardGamePage_Loaded;
        
        SaveButton.Text = updatingGame ? "Update" : "Save";
        NameTextField.IsReadOnly = updatingGame;
        NameTextField.BackgroundColor = updatingGame ? Colors.LightGray : Colors.White;
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