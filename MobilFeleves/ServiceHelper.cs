using Microsoft.Maui.Controls;

namespace MobilFeleves;

public static class ServiceHelper
{
    public static T GetService<T>() where T : notnull =>
        Current.GetService<T>() ?? throw new InvalidOperationException($"Service of type {typeof(T)} not found");

    private static IServiceProvider Current =>
        Application.Current?.Handler?.MauiContext?.Services
            ?? throw new InvalidOperationException("Service provider not initialized");
}
