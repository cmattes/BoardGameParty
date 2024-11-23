using BoardGameParty.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoardGameParty.ViewModels;

public class GameViewModel : ObservableObject
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _imageUri = string.Empty;
    private int _maximumNumberOfPlayers;
    private int _minimumNumberOfPlayers;
    private int _minutesPerGame;

    public GameViewModel(BoardGame game)
    {
        Name = game.Name;
        Description = game.Description;
        ImageUri = game.ImageUri;
        MaximumNumberOfPlayers = game.MaximumNumberOfPlayers;
        MinimumNumberOfPlayers = game.MinimumNumberOfPlayers;
        MinutesPerGame = game.MinutesPerGame;
    }
    
    public GameViewModel()
    {
        
    }

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