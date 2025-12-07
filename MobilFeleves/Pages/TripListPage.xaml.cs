using MobilFeleves.ViewModels;
using MobilFeleves;

namespace MobilFeleves.Pages;

public partial class TripListPage : ContentPage
{
    private readonly TripListViewModel _viewModel;

    public TripListPage() : this(ServiceHelper.GetService<TripListViewModel>())
    {
    }

    public TripListPage(TripListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTripsAsync();
    }
}
