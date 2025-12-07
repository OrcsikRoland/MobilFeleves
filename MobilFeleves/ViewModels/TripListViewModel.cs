using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

public class TripListViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private Trip? _selectedTrip;

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

    public Command LoadTripsCommand { get; }
    public Command AddTripCommand { get; }

    public TripListViewModel(ITripRepository repository)
    {
        _repository = repository;
        Title = "Túrák";

        LoadTripsCommand = new Command(async () => await LoadTripsAsync());
        AddTripCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Pages.TripEditPage)));
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
}
