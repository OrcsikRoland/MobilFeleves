using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

public class TripListViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private readonly IConnectivityService _connectivityService;
    private Trip? _selectedTrip;
    private bool _isOnline;
    private string _connectivityMessage = string.Empty;

    public ObservableCollection<Trip> Trips { get; } = new();

    public Trip? SelectedTrip
    {
        get => _selectedTrip;
        set
        {
            if (SetProperty(ref _selectedTrip, value) && value is not null)
            {
                OnTripTapped(value);
            }
        }
    }

    public bool IsOnline
    {
        get => _isOnline;
        set => SetProperty(ref _isOnline, value);
    }

    public string ConnectivityMessage
    {
        get => _connectivityMessage;
        set => SetProperty(ref _connectivityMessage, value);
    }

    public Command LoadTripsCommand { get; }
    public Command AddTripCommand { get; }

    public TripListViewModel(ITripRepository repository, IConnectivityService connectivityService)
    {
        _repository = repository;
        _connectivityService = connectivityService;
        Title = "Túrák";

        LoadTripsCommand = new Command(async () => await LoadTripsAsync());
        AddTripCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Pages.TripEditPage)));

        UpdateConnectivity(_connectivityService.IsConnected);
        _connectivityService.ConnectivityChanged += (_, isConnected) => UpdateConnectivity(isConnected);
    }

    public async Task LoadTripsAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Trips.Clear();
            var trips = await _repository.GetTripsAsync();
            foreach (var trip in trips)
            {
                Trips.Add(trip);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnTripTapped(Trip trip)
    {
        if (trip is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(Pages.TripDetailPage)}?tripId={trip.Id}");
    }

    private void UpdateConnectivity(bool isConnected)
    {
        IsOnline = isConnected;
        ConnectivityMessage = isConnected
            ? "Online: megosztás és frissítés elérhető"
            : "Offline: a megosztás letiltva";
    }
}
