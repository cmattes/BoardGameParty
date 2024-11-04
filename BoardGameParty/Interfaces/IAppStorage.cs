namespace BoardGameParty.Interfaces;

public interface IAppStorage 
{
    Task<IList<BoardGame>> SetupLocalStorage();
    Task SaveToCloud();
    Task<IList<BoardGame>> LoadFromCloud();
    Task SaveLocalData(IList<BoardGame> boardGames);
    Task<IList<BoardGame>> LoadLocalData();
    
}