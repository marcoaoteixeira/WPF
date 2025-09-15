﻿using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.Client.ViewModels.Pages;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.Navigation;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace Nameless.WPF.Client.Views.Pages;

[ServiceLifetime(Lifetime = ServiceLifetime.Singleton)]
[NavigationViewItem(Icon = SymbolRegular.Settings48, Title = "Configurações", Footer = true)]
public partial class AppConfigurationPage : INavigableView<AppConfigurationPageViewModel> {
    public AppConfigurationPageViewModel ViewModel { get; }

    public AppConfigurationPage(AppConfigurationPageViewModel viewModel) {
        ViewModel = Guard.Against.Null(viewModel);

        DataContext = ViewModel;

        InitializeComponent();
    }
}
