using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MobilFeleves.Pages;
using MobilFeleves.Services;
using MobilFeleves.ViewModels;

namespace MobilFeleves;

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

        builder.Services.AddSingleton<ITripRepository, TripRepository>();

        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<TripListViewModel>();
        builder.Services.AddTransient<TripDetailViewModel>();
        builder.Services.AddTransient<TripEditViewModel>();

        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<TripListPage>();
        builder.Services.AddTransient<TripDetailPage>();
        builder.Services.AddTransient<TripEditPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
