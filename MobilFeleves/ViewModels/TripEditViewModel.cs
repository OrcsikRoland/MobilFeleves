using System.Globalization;
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
    private string _distanceKmText = string.Empty;
    private string _elevationGainText = string.Empty;
    private string _durationMinutesText = string.Empty;
    private string _startLatitudeText = string.Empty;
    private string _startLongitudeText = string.Empty;

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

    public string DistanceKmText
    {
        get => _distanceKmText;
        set => SetProperty(ref _distanceKmText, value);
    }

    public string ElevationGainText
    {
        get => _elevationGainText;
        set => SetProperty(ref _elevationGainText, value);
    }

    public string DurationMinutesText
    {
        get => _durationMinutesText;
        set => SetProperty(ref _durationMinutesText, value);
    }

    public string StartLatitudeText
    {
        get => _startLatitudeText;
        set => SetProperty(ref _startLatitudeText, value);
    }

    public string StartLongitudeText
    {
        get => _startLongitudeText;
        set => SetProperty(ref _startLongitudeText, value);
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
            SyncFormFields();
            return;
        }

        var trip = await _repository.GetTripAsync(TripId);
        if (trip is not null)
        {
            Trip = trip;
            SyncFormFields();
        }
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Trip.Title))
        {
            ValidationMessage = "Adj meg egy nevet.";
            return;
        }

        if (!TryParseDouble(DistanceKmText, out var distanceKm))
        {
            ValidationMessage = "Érvénytelen táv (km).";
            return;
        }

        if (!TryParseInt(ElevationGainText, out var elevationGain))
        {
            ValidationMessage = "Érvénytelen szintemelkedés (m).";
            return;
        }

        if (!TryParseInt(DurationMinutesText, out var durationMinutes))
        {
            ValidationMessage = "Érvénytelen időtartam (perc).";
            return;
        }

        if (!TryParseDouble(StartLatitudeText, out var startLatitude))
        {
            ValidationMessage = "Érvénytelen kezdő szélesség.";
            return;
        }

        if (!TryParseDouble(StartLongitudeText, out var startLongitude))
        {
            ValidationMessage = "Érvénytelen kezdő hosszúság.";
            return;
        }

        Trip.DistanceKm = distanceKm;
        Trip.ElevationGainM = elevationGain;
        Trip.DurationMinutes = durationMinutes;
        Trip.StartLatitude = startLatitude;
        Trip.StartLongitude = startLongitude;

        ValidationMessage = string.Empty;
        await _repository.SaveTripAsync(Trip);
        await Shell.Current.GoToAsync("..", true);
    }

    private void SyncFormFields()
    {
        DistanceKmText = Trip.DistanceKm == 0 ? string.Empty : Trip.DistanceKm.ToString("0.##", CultureInfo.InvariantCulture);
        ElevationGainText = Trip.ElevationGainM == 0 ? string.Empty : Trip.ElevationGainM.ToString(CultureInfo.InvariantCulture);
        DurationMinutesText = Trip.DurationMinutes == 0 ? string.Empty : Trip.DurationMinutes.ToString(CultureInfo.InvariantCulture);
        StartLatitudeText = Trip.StartLatitude == 0 ? string.Empty : Trip.StartLatitude.ToString("0.######", CultureInfo.InvariantCulture);
        StartLongitudeText = Trip.StartLongitude == 0 ? string.Empty : Trip.StartLongitude.ToString("0.######", CultureInfo.InvariantCulture);
    }

    private static bool TryParseDouble(string? input, out double value)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = 0;
            return true;
        }

        return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static bool TryParseInt(string? input, out int value)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = 0;
            return true;
        }

        return int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
    }
}
