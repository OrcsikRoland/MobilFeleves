using Microsoft.Maui.Controls;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private int _tripCount;
    private double _totalDistance;

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

    public Command NavigateToListCommand { get; }
    public Command AddTripCommand { get; }

    public DashboardViewModel(ITripRepository repository)
    {
        _repository = repository;
        Title = "HikeMate";
        NavigateToListCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Pages.TripListPage)));
        AddTripCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Pages.TripEditPage)));
    }

    public async Task LoadSummaryAsync()
    {
        var trips = await _repository.GetTripsAsync();
        TripCount = trips.Count;
        TotalDistance = trips.Sum(t => t.DistanceKm);
    }
}
