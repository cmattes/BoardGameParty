namespace BoardGameParty.Models;

public record BoardGame(
    string Name,
    string Description,
    string ImageUri,
    int MinimumNumberOfPlayers,
    int MaximumNumberOfPlayers,
    int MinutesPerGame);