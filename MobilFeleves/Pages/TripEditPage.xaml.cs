using MobilFeleves.ViewModels;
using MobilFeleves;

namespace MobilFeleves.Pages;

public partial class TripEditPage : ContentPage
{
    private readonly TripEditViewModel _viewModel;

    public TripEditPage() : this(ServiceHelper.GetService<TripEditViewModel>())
    {
    }

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
