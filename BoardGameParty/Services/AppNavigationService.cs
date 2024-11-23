using BoardGameParty.Interfaces;
using BoardGameParty.Views;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.Services;

public class AppNavigationService : IAppNavigationService
{
    private readonly ILogger<AppNavigationService> _logger;
    private readonly IAppAlertService _appAlertService;

    public AppNavigationService(ILogger<AppNavigationService> logger, IAppAlertService appAlertService)
    {
        _logger = logger;
        _appAlertService = appAlertService;
    }

    public async Task NavigateTo(string pageName, bool pageParameter)
    {
        ContentPage? page = pageName switch
        {
            "SaveBoardGamePage" => new SaveBoardGamePage(pageParameter, this, _appAlertService),
            _ => null
        };

        if (page is null)
        {
            _logger.LogError($"Unable to navigate to page: {pageName}");
            return;
        }

        await Shell.Current.Navigation.PushAsync(page);
    }

    public async Task ReturnToRoot()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }
}