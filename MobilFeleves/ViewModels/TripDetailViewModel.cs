using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

[QueryProperty(nameof(TripId), "tripId")]
public class TripDetailViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private readonly IConnectivityService _connectivityService;
    private int _tripId;
    private Trip? _trip;
    private bool _isOnline;
    private string _connectivityMessage = string.Empty;

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

    public Command EditCommand { get; }
    public Command DeleteCommand { get; }
    public Command ShareCommand { get; }

    public TripDetailViewModel(ITripRepository repository, IConnectivityService connectivityService)
    {
        _repository = repository;
        _connectivityService = connectivityService;
        Title = "Túra részletei";

        EditCommand = new Command(async () => await GoToEditAsync(), () => Trip is not null);
        DeleteCommand = new Command(async () => await DeleteAsync(), () => Trip is not null);
        ShareCommand = new Command(async () => await ShareAsync(), () => Trip is not null && IsOnline);

        UpdateConnectivity(connectivityService.IsConnected);
        _connectivityService.ConnectivityChanged += (_, isConnected) => UpdateConnectivity(isConnected);
    }

    public async Task LoadTripAsync()
    {
        if (TripId == 0)
        {
            return;
        }

        Trip = await _repository.GetTripAsync(TripId);
        OnPropertyChanged(nameof(Trip));
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

    private void UpdateConnectivity(bool isConnected)
    {
        IsOnline = isConnected;
        ConnectivityMessage = isConnected
            ? "Online: a megosztás aktív"
            : "Offline: nincs internetkapcsolat";
        ShareCommand.ChangeCanExecute();
    }
}
