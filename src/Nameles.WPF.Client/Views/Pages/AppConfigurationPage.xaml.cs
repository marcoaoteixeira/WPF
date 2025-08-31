using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.Client.ViewModels.Pages;
using Nameless.WPF.DependencyInjection;
using Wpf.Ui.Abstractions.Controls;

namespace Nameless.WPF.Client.Views.Pages;

[ServiceLifetime(Lifetime = ServiceLifetime.Singleton)]
public partial class AppConfigurationPage : INavigableView<AppConfigurationPageViewModel> {
    public AppConfigurationPageViewModel ViewModel { get; }

    public AppConfigurationPage(AppConfigurationPageViewModel viewModel) {
        ViewModel = Guard.Against.Null(viewModel);

        DataContext = ViewModel;

        InitializeComponent();
    }
}
