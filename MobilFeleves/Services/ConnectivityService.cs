using Microsoft.Maui.Networking;

namespace MobilFeleves.Services;

public class ConnectivityService : IConnectivityService
{
    public bool IsConnected => IsNetworkAccessOnline(Connectivity.Current.NetworkAccess);

    public event EventHandler<bool>? ConnectivityChanged;

    public ConnectivityService()
    {
        Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
    }

    private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        var isOnline = IsNetworkAccessOnline(e.NetworkAccess);
        ConnectivityChanged?.Invoke(this, isOnline);
    }

    private static bool IsNetworkAccessOnline(NetworkAccess access) =>
        access is NetworkAccess.Internet or NetworkAccess.ConstrainedInternet;
}
