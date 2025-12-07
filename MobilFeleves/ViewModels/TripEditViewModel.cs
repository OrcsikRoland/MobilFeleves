using Microsoft.Maui.Controls;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

[QueryProperty(nameof(TripId), "tripId")]
public class TripEditViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private int _tripId;
    private Trip _trip = new();
    private string _validationMessage = string.Empty;

    public int TripId
    {
        get => _tripId;
        set => SetProperty(ref _tripId, value);
    }

    public Trip Trip
    {
        get => _trip;
        set => SetProperty(ref _trip, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetProperty(ref _validationMessage, value);
    }

    public Command SaveCommand { get; }

    public TripEditViewModel(ITripRepository repository)
    {
        _repository = repository;
        Title = "Túra szerkesztése";
        Trip.Date = DateTime.Today;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task LoadAsync()
    {
        if (TripId == 0)
        {
            Trip = new Trip { Date = DateTime.Today };
            return;
        }

        var trip = await _repository.GetTripAsync(TripId);
        if (trip is not null)
        {
            Trip = trip;
        }
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Trip.Title))
        {
            ValidationMessage = "Adj meg egy nevet.";
            return;
        }

        ValidationMessage = string.Empty;
        await _repository.SaveTripAsync(Trip);
        await Shell.Current.GoToAsync("..", true);
    }
}
