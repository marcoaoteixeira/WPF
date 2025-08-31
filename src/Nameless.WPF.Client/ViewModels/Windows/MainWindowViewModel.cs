using Nameless.Infrastructure;
using Nameless.WPF.UI.Mvvm;
using Wpf.Ui.Controls;

namespace Nameless.WPF.Client.ViewModels.Windows;

public sealed class MainWindowViewModel : ViewModel {
    private readonly IApplicationContext _applicationContext;

    private readonly Lazy<string> _appTitle;
    private readonly Lazy<string> _appVersion;
    private readonly Lazy<NavigationViewItem[]> _mainMenuItemsSource;
    private readonly Lazy<NavigationViewItem[]> _footerMenuItemsSource;

    public string AppTitle => _appTitle.Value;
    public string AppVersion => _appVersion.Value;
    public NavigationViewItem[] MainMenuItemsSource => _mainMenuItemsSource.Value;
    public NavigationViewItem[] FooterMenuItemsSource => _footerMenuItemsSource.Value;

    public MainWindowViewModel(IApplicationContext applicationContext) {
        _applicationContext = Guard.Against.Null(applicationContext);

        _appTitle = new Lazy<string>(GetAppTitle);
        _appVersion = new Lazy<string>(GetAppVersion);
        _mainMenuItemsSource = new Lazy<NavigationViewItem[]>(GetMainMenuItemsSource);
        _footerMenuItemsSource = new Lazy<NavigationViewItem[]>(GetFooterMenuItemsSource);
    }

    private static NavigationViewItem[] GetMainMenuItemsSource() {
        return [];
    }

    private static NavigationViewItem[] GetFooterMenuItemsSource() {
        return [
            new NavigationViewItem {
                Content = StaticData.Pages.AppConfiguration.Name,
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings48 },
                TargetPageType = StaticData.Pages.AppConfiguration.Type,
                ToolTip = StaticData.Pages.AppConfiguration.ToolTip
            }
        ];
    }

    private string GetAppTitle() {
        return _applicationContext.ApplicationName;
    }

    private string GetAppVersion() {
        return _applicationContext.Version;
    }
}