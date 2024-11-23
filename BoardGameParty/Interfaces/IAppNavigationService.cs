namespace BoardGameParty.Interfaces;

public interface IAppNavigationService
{
    Task NavigateTo(string pageName, bool pageParameter);
    Task ReturnToRoot();
}