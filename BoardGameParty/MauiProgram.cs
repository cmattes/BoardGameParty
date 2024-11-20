using CommunityToolkit.Maui;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using BoardGameParty.ViewModels;
using BoardGameParty.Views;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BoardGameParty;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "BoardGameLogs/log.txt"),
                    rollingInterval: RollingInterval.Day)
                .CreateLogger());

        builder.Services.AddSingleton<System.IO.Abstractions.IFileSystem, System.IO.Abstractions.FileSystem>();
        builder.Services.AddSingleton<IAppStorage>(provider => new AppStorage(
            provider.GetRequiredService<System.IO.Abstractions.IFileSystem>(),
            provider.GetRequiredService<ILogger<AppStorage>>(),
            Path.Combine(FileSystem.Current.AppDataDirectory, "BoardGameData")));

        builder.Services.AddSingleton<BoardGamesViewModel>();
        builder.Services.AddTransient<BoardGamesPage>(provider => new BoardGamesPage()
        {
            BindingContext = provider.GetRequiredService<BoardGamesViewModel>()
        });

        return builder.Build();
    }
}