using MobilFeleves.ViewModels;
using MobilFeleves;

namespace MobilFeleves.Pages;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;

    public TripDetailPage() : this(ServiceHelper.GetService<TripDetailViewModel>())
    {
    }

    public TripDetailPage(TripDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTripAsync();
    }
}
