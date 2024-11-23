using System.Windows.Input;
using BoardGameParty.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoardGameParty.ViewModels;

public class SaveBoardGameViewModel : ObservableObject
{
    private readonly bool _isUpdating;
    private readonly IAppNavigationService _navigationService;
    private GameViewModel? _changingGame;

    public SaveBoardGameViewModel(bool isUpdating, IAppNavigationService navigationService)
    {
        _isUpdating = isUpdating;
        _navigationService = navigationService;
        CancelCommand = new AsyncRelayCommand(CancelSave);
        SaveCommand = new AsyncRelayCommand(FinishSave, CanFinishSave);
        TextUpdatedCommand = new RelayCommand<TextChangedEventArgs>(CanSaveExecute);
        GetImageCommand = new RelayCommand(PickImage);
    }

    public GameViewModel? ChangingGame
    {
        get => _changingGame;
        set => SetProperty(ref _changingGame, value);
    }

    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }
    public ICommand TextUpdatedCommand { get; }
    public ICommand GetImageCommand { get; }

    private async Task CancelSave()
    {
        if (ChangingGame is not null && (!string.IsNullOrWhiteSpace(ChangingGame.Name) ||
                                         !string.IsNullOrWhiteSpace(ChangingGame.Description) ||
                                         !string.IsNullOrWhiteSpace(ChangingGame.ImageUri)
                                         || ChangingGame.MinimumNumberOfPlayers > 0 ||
                                         ChangingGame.MaximumNumberOfPlayers > 0 || ChangingGame.MinutesPerGame > 0))
        {
            var cancel = await Shell.Current.DisplayAlert("Cancel",
                $"You have unsaved changes.{Environment.NewLine}Continue to Cancel?",
                "Yes", "No");

            if (!cancel) return;
        }

        await _navigationService.ReturnToRoot();
    }

    private void CanSaveExecute(TextChangedEventArgs args)
    {
        SaveCommand.NotifyCanExecuteChanged();
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
        }

        await _navigationService.ReturnToRoot();
    }
    
    private void PickImage()
    {
        
    }
}