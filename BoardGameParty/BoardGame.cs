namespace BoardGameParty;

public class BoardGame
{
    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public string GameImageURI { get; set; }
    public (int min, int max) NumberOfPlayers { get; set; }
    public int NumberOfGames { get; set; }
    public int MinutesPerGame { get; set; }
}