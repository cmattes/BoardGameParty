using BoardGameParty.Interfaces;

namespace BoardGameParty.Services;

public class AppAlertService : IAppAlertService
{
    public async Task<bool> ShowAlert(string title, string message, string acceptWording, string cancelWording)
    {
        return await Shell.Current.DisplayAlert(title, message, acceptWording, cancelWording);
    }
}