namespace BoardGameParty.Interfaces;

public interface IAppAlertService
{
    Task<bool> ShowAlert(string title, string message, string acceptWording, string cancelWording);
}