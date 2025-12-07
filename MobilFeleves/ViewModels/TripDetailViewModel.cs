using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

[QueryProperty(nameof(TripId), "tripId")]
public class TripDetailViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private static readonly Location DefaultStartLocation = new(47.4979, 19.0402);
    private int _tripId;
    private Trip? _trip;
    private Location _startLocation = DefaultStartLocation;
    private MapSpan _mapRegion;

    public int TripId
    {
        get => _tripId;
        set => SetProperty(ref _tripId, value);
    }

    public Trip? Trip
    {
        get => _trip;
        set => SetProperty(ref _trip, value);
    }

    public Location StartLocation
    {
        get => _startLocation;
        set => SetProperty(ref _startLocation, value);
    }

    public MapSpan MapRegion
    {
        get => _mapRegion;
        set => SetProperty(ref _mapRegion, value);
    }

    public Command EditCommand { get; }
    public Command DeleteCommand { get; }
    public Command ShareCommand { get; }

    public TripDetailViewModel(ITripRepository repository)
    {
        _repository = repository;
        Title = "Túra részletei";

        EditCommand = new Command(async () => await GoToEditAsync(), () => Trip is not null);
        DeleteCommand = new Command(async () => await DeleteAsync(), () => Trip is not null);
        ShareCommand = new Command(async () => await ShareAsync(), () => Trip is not null);

        _mapRegion = MapSpan.FromCenterAndRadius(StartLocation, Distance.FromKilometers(5));
    }

    public async Task LoadTripAsync()
    {
        if (TripId == 0)
        {
            return;
        }

        Trip = await _repository.GetTripAsync(TripId);
        OnPropertyChanged(nameof(Trip));
        UpdateLocation();
        UpdateCommands();
    }

    private void UpdateCommands()
    {
        EditCommand.ChangeCanExecute();
        DeleteCommand.ChangeCanExecute();
        ShareCommand.ChangeCanExecute();
    }

    private async Task GoToEditAsync()
    {
        if (Trip is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(Pages.TripEditPage)}?tripId={Trip.Id}");
    }

    private async Task DeleteAsync()
    {
        if (Trip is null)
        {
            return;
        }

        await _repository.DeleteTripAsync(Trip);
        await Shell.Current.GoToAsync("..", true);
    }

    private async Task ShareAsync()
    {
        if (Trip is null)
        {
            return;
        }

        var text = $"{Trip.Title}\n{Trip.DistanceKm:F1} km, {Trip.ElevationGainM} m szint\nIdőtartam: {Trip.DurationText}\nDátum: {Trip.Date:d}";
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Subject = "Túra összefoglaló",
            Text = text
        });
    }

    private void UpdateLocation()
    {
        var latitude = Trip?.StartLatitude;
        var longitude = Trip?.StartLongitude;

        var location = latitude is null or 0 || longitude is null or 0
            ? DefaultStartLocation
            : new Location(latitude.Value, longitude.Value);

        StartLocation = location;
        MapRegion = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
    }
}
