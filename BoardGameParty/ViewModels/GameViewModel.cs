using BoardGameParty.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoardGameParty.ViewModels;

public class GameViewModel(BoardGame game) : ObservableObject
{
    private string _description = game.Description;
    private string _imageUri = game.ImageUri;
    private int _maximumNumberOfPlayers = game.MaximumNumberOfPlayers;
    private int _minimumNumberOfPlayers = game.MinimumNumberOfPlayers;
    private int _minutesPerGame = game.MinutesPerGame;
    private string _name = game.Name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string ImageUri
    {
        get => _imageUri;
        set => SetProperty(ref _imageUri, value);
    }

    public int MinimumNumberOfPlayers
    {
        get => _minimumNumberOfPlayers;
        set => SetProperty(ref _minimumNumberOfPlayers, value);
    }

    public int MaximumNumberOfPlayers
    {
        get => _maximumNumberOfPlayers;
        set => SetProperty(ref _maximumNumberOfPlayers, value);
    }

    public int MinutesPerGame
    {
        get => _minutesPerGame;
        set => SetProperty(ref _minutesPerGame, value);
    }
}