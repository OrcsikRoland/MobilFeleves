using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MobilFeleves.ViewModels;

namespace MobilFeleves.Pages;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;

    public TripDetailPage(TripDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTripAsync();
        UpdateMap();
    }

    private void UpdateMap()
    {
        TripMap.Pins.Clear();

        var hasTrip = _viewModel.Trip is not null;
        var pinLocation = _viewModel.StartLocation;
        var pinLabel = hasTrip ? _viewModel.Trip!.Title : "Túra kezdőpont";
        var pinAddress = hasTrip && _viewModel.Trip!.DistanceKm > 0
            ? $"{_viewModel.Trip.DistanceKm:F1} km túra"
            : "Kezdő helyszín";

        TripMap.Pins.Add(new Pin
        {
            Label = pinLabel,
            Address = pinAddress,
            Type = PinType.Place,
            Location = pinLocation
        });

        TripMap.MoveToRegion(_viewModel.MapRegion ?? MapSpan.FromCenterAndRadius(pinLocation, Distance.FromKilometers(5)));
    }
}
