using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Client.Internals;
using Nameless.WPF.Client.Resources;
using Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;
using Nameless.WPF.Client.ViewModels.Windows;
using Nameless.WPF.Configuration;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.Dialogs.MessageBox;
using Nameless.WPF.Mvvm;
using Nameless.WPF.Notifications;
using Nameless.WPF.Snackbar;
using Nameless.WPF.UseCases.SystemUpdate.Check;
using Nameless.WPF.UseCases.SystemUpdate.Download;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Nameless.WPF.Client.Views.Windows;

[ServiceLifetime(Lifetime = ServiceLifetime.Singleton)]
public partial class MainWindow : INavigationWindow, IHasViewModel<MainWindowViewModel> {
    private readonly IAppConfigurationManager _appConfigurationManager;
    private readonly IContentDialogService _contentDialogService;
    private readonly IMessageBox _messageBox;
    private readonly INavigationService _navigationService;
    private readonly INavigationViewPageProvider _navigationViewPageProvider;
    private readonly INotificationService _notificationService;
    private readonly ISnackbarService _snackbarService;
    private readonly ILogger<MainWindow> _logger;

    private bool _initialized;

    public MainWindowViewModel ViewModel { get; }

    public MainWindow(
        MainWindowViewModel viewModel,
        IAppConfigurationManager appConfigurationManager,
        IContentDialogService contentDialogService,
        IMessageBox messageBox,
        INavigationService navigationService,
        INavigationViewPageProvider navigationViewPageProvider,
        INotificationService notificationService,
        ISnackbarService snackbarService,
        ILogger<MainWindow> logger) {

        ViewModel = Guard.Against.Null(viewModel);
        DataContext = ViewModel;

        _appConfigurationManager = Guard.Against.Null(appConfigurationManager);
        _contentDialogService = Guard.Against.Null(contentDialogService);
        _messageBox = Guard.Against.Null(messageBox);
        _navigationService = Guard.Against.Null(navigationService);
        _navigationViewPageProvider = Guard.Against.Null(navigationViewPageProvider);
        _notificationService = Guard.Against.Null(notificationService);
        _snackbarService = Guard.Against.Null(snackbarService);
        _logger = Guard.Against.Null(logger);

        InitializeComponent();
        InitializeWindow();
    }

    public INavigationView GetNavigation() {
        return NavigationViewRoot;
    }

    public bool Navigate(Type pageType) {
        return GetNavigation().Navigate(pageType);
    }

    public void SetServiceProvider(IServiceProvider serviceProvider) {
        throw new NotImplementedException();
    }

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) {
        GetNavigation().SetPageProviderService(navigationViewPageProvider);
    }

    public void ShowWindow() {
        Show();
    }

    public void CloseWindow() {
        Close();
    }

    private void ClosingHandler(object? _, CancelEventArgs args) {
        if (!_appConfigurationManager.GetConfirmBeforeExit()) {
            return;
        }

        var result = _messageBox.ShowQuestion(
            title: Strings.MainWindow_ConfirmApplicationExit_MessageBox_Title,
            message: Strings.MainWindow_ConfirmApplicationExit_MessageBox_Message,
            buttons: MessageBoxButtons.YesNoCancel);

        if (result == MessageBoxResult.No) {
            _appConfigurationManager.SetConfirmBeforeExit(false);
        }

        args.Cancel = result == MessageBoxResult.Cancel;
    }

    private void InitializeWindow() {
        if (_initialized) { return; }

        SetApplicationTheme();
        SetContentPresenter();
        SetNavigationView();
        SetPageService(_navigationViewPageProvider);
        SetSnackbarPresenter();
        SetWindowIcon();
        SubscribeForNotifications();

        _initialized = true;
    }

    private void SetApplicationTheme() {
        SystemThemeWatcher.Watch(this);
        var currentTheme = _appConfigurationManager.GetTheme();
        ApplicationThemeManager.Apply(currentTheme.ToApplicationTheme());
    }

    private void SetContentPresenter() {
        _contentDialogService.SetDialogHost(ContentPresenterRoot);
    }

    private void SetNavigationView() {
        _navigationService.SetNavigationControl(NavigationViewRoot);
    }

    private void SetSnackbarPresenter() {
        _snackbarService.SetSnackbarPresenter(SnackbarPresenterRoot);
    }

    private void SetWindowIcon() {
        try { Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/application_64x64.png")); }
        catch (Exception ex) { _logger.SetWindowIconFailure(ex); }
    }

    private void SubscribeForNotifications() {
        _notificationService.Subscribe<PerformDatabaseBackupNotification>(this, ShowNotificationInSnackbar);
        _notificationService.Subscribe<CheckForUpdateNotification>(this, ShowNotificationInSnackbar);
        _notificationService.Subscribe<DownloadUpdateNotification>(this, ShowNotificationInSnackbar);

    }

    private void ShowNotificationInSnackbar(object sender, INotification notification) {
        Dispatcher.InvokeAsync(() => _snackbarService.Show(notification.ToSnackbarParameters()));
    }
}