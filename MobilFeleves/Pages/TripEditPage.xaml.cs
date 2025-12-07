using MobilFeleves.ViewModels;

namespace MobilFeleves.Pages;

public partial class TripEditPage : ContentPage
{
    private readonly TripEditViewModel _viewModel;

    public TripEditPage(TripEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
