using MobilFeleves.ViewModels;
using MobilFeleves;

namespace MobilFeleves.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardPage() : this(ServiceHelper.GetService<DashboardViewModel>())
    {
    }

    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSummaryAsync();
    }
}
