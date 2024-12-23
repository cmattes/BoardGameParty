using System.Windows.Input;
using BoardGameParty.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoardGameParty.ViewModels;

public class SaveBoardGameViewModel : ObservableObject
{
    private readonly IAppAlertService _alertService;
    private readonly GameViewModel? _changingGame;
    private readonly bool _isUpdating;
    private readonly IAppNavigationService _navigationService;

    public SaveBoardGameViewModel(bool isUpdating, IAppNavigationService navigationService,
        IAppAlertService alertService)
    {
        _isUpdating = isUpdating;
        _navigationService = navigationService;
        _alertService = alertService;
        CancelCommand = new AsyncRelayCommand(CancelSave);
        SaveCommand = new AsyncRelayCommand(FinishSave, CanFinishSave);
        TextUpdatedCommand = new RelayCommand<TextChangedEventArgs>(CanSaveExecute);
        GetImageCommand = new RelayCommand(PickImage);
    }

    public GameViewModel? ChangingGame
    {
        get => _changingGame;
        init => SetProperty(ref _changingGame, value);
    }

    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }
    public ICommand TextUpdatedCommand { get; }
    public ICommand GetImageCommand { get; }

    private async Task CancelSave()
    {
        if (ChangingGame is not null && _isUpdating)
        {
            var selected = App.BoardGamesListViewModel.SelectedGame;
            if (selected is not null && (!ChangingGame.Description.Equals(selected.Description)
                                         || !ChangingGame.ImageUri.Equals(selected.ImageUri)
                                         || !ChangingGame.MinutesPerGame.Equals(selected.MinutesPerGame)
                                         || !ChangingGame.MinimumNumberOfPlayers.Equals(selected.MinimumNumberOfPlayers)
                                         || !ChangingGame.MaximumNumberOfPlayers.Equals(selected
                                             .MaximumNumberOfPlayers)))
            {
                var cancelConfirmed = await _alertService.ShowAlert("Cancel Save",
                    $"You have unsaved changes.{Environment.NewLine}Continue to Cancel?",
                    "Yes", "No");

                if (!cancelConfirmed) return;
            }
        }
        else if (ChangingGame is not null && (!string.IsNullOrWhiteSpace(ChangingGame.Name) ||
                                              !string.IsNullOrWhiteSpace(ChangingGame.Description) ||
                                              !string.IsNullOrWhiteSpace(ChangingGame.ImageUri)
                                              || ChangingGame.MinimumNumberOfPlayers > 0 ||
                                              ChangingGame.MaximumNumberOfPlayers > 0 ||
                                              ChangingGame.MinutesPerGame > 0))
        {
            var cancelConfirmed = await _alertService.ShowAlert("Cancel Save",
                $"You have unsaved changes.{Environment.NewLine}Continue to Cancel?",
                "Yes", "No");

            if (!cancelConfirmed) return;
        }

        await _navigationService.ReturnToRoot();
    }

    private bool CanFinishSave()
    {
        if (ChangingGame is null || string.IsNullOrWhiteSpace(ChangingGame.Name)) return false;

        if (!_isUpdating) return true;

        var selected = App.BoardGamesListViewModel.SelectedGame;
        if (selected is null) return false;

        return !ChangingGame.Description.Equals(selected.Description)
               || !ChangingGame.ImageUri.Equals(selected.ImageUri)
               || !ChangingGame.MinutesPerGame.Equals(selected.MinutesPerGame)
               || !ChangingGame.MinimumNumberOfPlayers.Equals(selected.MinimumNumberOfPlayers)
               || !ChangingGame.MaximumNumberOfPlayers.Equals(selected.MaximumNumberOfPlayers);
    }

    private async Task FinishSave()
    {
        //Check that the Name is Unique
        if (ChangingGame is not null)
        {
            if (string.IsNullOrWhiteSpace(ChangingGame.ImageUri))
            {
                ChangingGame.ImageUri = "no_image.png";
            }
            
            var game = App.CloneGameViewModel(ChangingGame);
            if (game is not null && _isUpdating)
            {
                App.BoardGamesListViewModel.BoardGames.Remove(
                    App.BoardGamesListViewModel.BoardGames.First(model => model.Name == ChangingGame.Name));
                App.BoardGamesListViewModel.BoardGames.Add(game);
            }
            else if (game is not null)
            {
                App.BoardGamesListViewModel.BoardGames.Add(game);
            }
            
            App.BoardGamesListViewModel.SelectedGame = App.BoardGamesListViewModel.BoardGames.First(model => model.Name == ChangingGame.Name);

            await App.BoardGamesListViewModel.SaveBoardGames();
        }

        await _navigationService.ReturnToRoot();
    }

    private void CanSaveExecute(TextChangedEventArgs? args)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    private void PickImage()
    {
    }
}