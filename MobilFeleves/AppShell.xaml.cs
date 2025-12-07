namespace MobilFeleves;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Pages.TripDetailPage), typeof(Pages.TripDetailPage));
        Routing.RegisterRoute(nameof(Pages.TripEditPage), typeof(Pages.TripEditPage));
    }
}
