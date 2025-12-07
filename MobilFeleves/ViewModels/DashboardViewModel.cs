using Microsoft.Maui.Controls;
using MobilFeleves.Pages;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private readonly IConnectivityService _connectivityService;
    private int _tripCount;
    private double _totalDistance;
    private string _connectivityMessage = string.Empty;

    public int TripCount
    {
        get => _tripCount;
        set => SetProperty(ref _tripCount, value);
    }

    public double TotalDistance
    {
        get => _totalDistance;
        set => SetProperty(ref _totalDistance, value);
    }

    public string ConnectivityMessage
    {
        get => _connectivityMessage;
        set => SetProperty(ref _connectivityMessage, value);
    }

    public Command NavigateToListCommand { get; }
    public Command AddTripCommand { get; }

    public DashboardViewModel(ITripRepository repository, IConnectivityService connectivityService)
    {
        _repository = repository;
        _connectivityService = connectivityService;
        Title = "HikeMate";
        NavigateToListCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(Pages.TripListPage)}"));
        AddTripCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Pages.TripEditPage)));

        UpdateConnectivity(connectivityService.IsConnected);
        _connectivityService.ConnectivityChanged += (_, isConnected) => UpdateConnectivity(isConnected);
    }

    public async Task LoadSummaryAsync()
    {
        var trips = await _repository.GetTripsAsync();
        TripCount = trips.Count;
        TotalDistance = trips.Sum(t => t.DistanceKm);
    }

    private void UpdateConnectivity(bool isConnected)
    {
        ConnectivityMessage = isConnected
            ? "Online: a megosztás és szinkron funkciók elérhetők"
            : "Offline: ellenőrizd a hálózatot";
    }
}
