using Microsoft.Maui.Controls.Maps;
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
        if (_viewModel.Trip is null)
        {
            return;
        }

        TripMap.Pins.Clear();

        TripMap.Pins.Add(new Pin
        {
            Label = _viewModel.Trip.Title,
            Address = _viewModel.Trip.DistanceKm > 0
                ? $"{_viewModel.Trip.DistanceKm:F1} km túra"
                : "Túra kezdete",
            Type = PinType.Place,
            Location = _viewModel.StartLocation
        });

        TripMap.MoveToRegion(_viewModel.MapRegion);
    }
}
