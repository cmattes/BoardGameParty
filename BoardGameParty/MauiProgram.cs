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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        IServiceCollection services = builder.Services;

        services.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger());
        

        return builder.Build();
    }
}