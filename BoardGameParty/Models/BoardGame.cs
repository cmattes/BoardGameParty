namespace BoardGameParty.Models;

public class BoardGame(string name)
{
    public string Name { get; } = name;
    public string Description { get; set; } = string.Empty;
    public string ImageURI { get; set; } = string.Empty;
    public int MinimumNumberOfPlayers { get; set; }
    public int MaximumNumberOfPlayers { get; set; }
    public int MinutesPerGame { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not BoardGame)
        {
            return false;
        }
        
        var otherBoardGame = (BoardGame)obj;

        return Name == otherBoardGame.Name;
    }

    public static bool operator ==(BoardGame x, BoardGame y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(BoardGame x, BoardGame y)
    {
        return !x.Equals(y);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}