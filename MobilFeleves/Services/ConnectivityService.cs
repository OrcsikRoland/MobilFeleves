using Microsoft.Maui.Networking;

namespace MobilFeleves.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    public event EventHandler<bool>? ConnectivityChanged;

    public ConnectivityService()
    {
        Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
    }

    private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        var isOnline = e.NetworkAccess == NetworkAccess.Internet;
        ConnectivityChanged?.Invoke(this, isOnline);
    }
}
