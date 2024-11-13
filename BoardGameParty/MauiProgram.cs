using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace BoardGameParty;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
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

        builder.Services.AddSingleton<IFileSystem, System.IO.Abstractions.FileSystem>();
        builder.Services.AddSingleton<IAppStorage>(provider => new AppStorage(
            provider.GetRequiredService<IFileSystem>(),
            provider.GetRequiredService<ILogger<AppStorage>>(),
            Path.Combine(FileSystem.Current.AppDataDirectory, "BoardGameData")));

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<MainPage>(provider => new MainPage
        {
            BindingContext = provider.GetRequiredService<MainPageViewModel>()
        });

        return builder.Build();
    }
}