using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using MobilFeleves.Models;
using MobilFeleves.Services;

namespace MobilFeleves.ViewModels;

[QueryProperty(nameof(TripId), "tripId")]
public class TripDetailViewModel : BaseViewModel
{
    private readonly ITripRepository _repository;
    private int _tripId;
    private Trip? _trip;

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

    public Command EditCommand { get; }
    public Command DeleteCommand { get; }
    public Command ShareCommand { get; }
    public Command CapturePhotoCommand { get; }

    public TripDetailViewModel(ITripRepository repository)
    {
        _repository = repository;
        Title = "Túra részletei";

        EditCommand = new Command(async () => await GoToEditAsync(), () => Trip is not null);
        DeleteCommand = new Command(async () => await DeleteAsync(), () => Trip is not null);
        ShareCommand = new Command(async () => await ShareAsync(), () => Trip is not null);
        CapturePhotoCommand = new Command(async () => await CapturePhotoAsync(), () => Trip is not null);
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
        CapturePhotoCommand.ChangeCanExecute();
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

    private async Task CapturePhotoAsync()
    {
        if (Trip is null)
        {
            return;
        }

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await Shell.Current.DisplayAlert("Kamera", "A készüléken nem érhető el a kamera.", "OK");
            return;
        }

        var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
        {
            Title = $"{Trip.Title}-foto"
        });

        if (photo is null)
        {
            return;
        }

        var filePath = Path.Combine(FileSystem.AppDataDirectory,
            $"trip_{Trip.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.jpg");

        await using var sourceStream = await photo.OpenReadAsync();
        await using var targetStream = File.Create(filePath);
        await sourceStream.CopyToAsync(targetStream);

        Trip.PhotoPath = filePath;
        await _repository.SaveTripAsync(Trip);
        OnPropertyChanged(nameof(Trip));
    }
}
