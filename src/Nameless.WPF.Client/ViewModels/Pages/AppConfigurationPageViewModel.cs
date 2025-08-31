using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.Infrastructure;
using Nameless.Mediator;
using Nameless.WPF.Client.UseCases.Database.Backup;
using Nameless.WPF.Configuration;
using Nameless.WPF.UI;
using Nameless.WPF.UI.Dialogs.TaskRunnerDialog;
using Nameless.WPF.UI.Dialogs.UserDialog;
using Nameless.WPF.UI.Helpers;
using Nameless.WPF.UI.Mvvm;
using Nameless.WPF.UI.Windows;
using Nameless.WPF.UseCases.SystemUpdate.DownloadLatestVersion;
using Nameless.WPF.UseCases.SystemUpdate.VerifyNewVersion;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;
using SysDialogResult = System.Windows.Forms.DialogResult;
using SysMessageBox = System.Windows.Forms.MessageBox;
using SysMessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using SysMessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

namespace Nameless.WPF.Client.ViewModels.Pages;

public partial class AppConfigurationPageViewModel : ViewModel, INavigationAware {
    private readonly IAppConfigurationManager _appConfigurationManager;
    private readonly IApplicationContext _applicationContext;
    private readonly IMediator _mediator;
    private readonly IUserDialog _userDialog;
    private readonly IWindowFactory _windowFactory;

    private bool _initialized;

    [ObservableProperty]
    private ComboBoxItem _currentTheme = ComboBoxItemHelper.EmptyComboBoxItem;

    [ObservableProperty]
    private bool _currentConfirmBeforeExit;

    public string AppVersion { get; private set; } = string.Empty;

    public ComboBoxItem[] AvailableThemes { get; } = [
        Theme.Light.ToComboBoxItem(),
        Theme.Dark.ToComboBoxItem(),
        Theme.HighContrast.ToComboBoxItem()
    ];

    public AppConfigurationPageViewModel(
        IAppConfigurationManager appConfigurationManager,
        IApplicationContext applicationContext,
        IMediator mediator,
        IUserDialog userDialog,
        IWindowFactory windowFactory) {
        _appConfigurationManager = Guard.Against.Null(appConfigurationManager);
        _applicationContext = Guard.Against.Null(applicationContext);
        _mediator = Guard.Against.Null(mediator);
        _userDialog = Guard.Against.Null(userDialog);
        _windowFactory = Guard.Against.Null(windowFactory);
    }

    public Task OnNavigatedToAsync() {
        Initialize();

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync() {
        throw new NotImplementedException();
    }

    private void Initialize() {
        if (_initialized) { return; }

        AppVersion = _applicationContext.Version;

        var theme = _appConfigurationManager.GetTheme();

        CurrentTheme = theme.GetComboBoxItem(AvailableThemes);
        CurrentConfirmBeforeExit = _appConfigurationManager.GetConfirmBeforeExit();

        _initialized = true;
    }

    [RelayCommand]
    private Task PerformSystemUpdateAsync() {
        if (_windowFactory.TryCreate<ITaskRunnerDialogWindow>(out var window)) {
            window.SetTitle("Atualização do Sistema")
                  .SubscribeFor<VerifyNewVersionNotification>()
                  .SubscribeFor<DownloadLatestVersionNotification>()
                  .SetOwner(null)
                  .SetHandler(ExecuteSystemUpdateAsync)
                  .Show(WindowStartupLocation.CenterScreen);
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenApplicationDataDirectoryAsync() {
        ProcessHelper.OpenDirectory(_applicationContext.DataDirectoryPath);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenApplicationLogFileAsync() {
        ProcessHelper.OpenTextFile(Constants.Application.LogFileName);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task PerformApplicationDatabaseBackupAsync() {
        if (_windowFactory.TryCreate<ITaskRunnerDialogWindow>(out var window)) {
            window.SetTitle("Realizando backup da base de dados...")
                  .SubscribeFor<PerformDatabaseBackupNotification>()
                  .SetOwner(null)
                  .SetHandler(PerformDatabaseBackupAsync)
                  .Show(WindowStartupLocation.CenterScreen);
        }

        return Task.CompletedTask;
    }

    partial void OnCurrentThemeChanged(ComboBoxItem? oldValue, ComboBoxItem newValue) {
        if (!_initialized || oldValue?.Tag == newValue.Tag) { return; }

        var theme = (Theme)newValue.Tag;

        ApplicationThemeManager.Apply(theme.ToApplicationTheme());

        _appConfigurationManager.SetTheme(theme);
    }

    partial void OnCurrentConfirmBeforeExitChanged(bool oldValue, bool newValue) {
        if (!_initialized || oldValue == newValue) { return; }

        _appConfigurationManager.SetConfirmBeforeExit(newValue);
    }

    private async Task PerformDatabaseBackupAsync(CancellationToken cancellationToken) {
        var request = new PerformDatabaseBackupRequest();

        _ = await _mediator.ExecuteAsync(request, cancellationToken)
                           .SuppressContext();
    }

    private async Task ExecuteSystemUpdateAsync(CancellationToken cancellationToken) {
        var newVersionResponse = await ExecuteSystemUpdateVerificationAsync(cancellationToken).SuppressContext();

        if (!newVersionResponse.Succeeded) { return; }
        if (string.IsNullOrWhiteSpace(newVersionResponse.Version) || string.IsNullOrWhiteSpace(newVersionResponse.DownloadUrl)) { return; }

        const string Question = "Uma nova versão está disponível para download. Deseja fazer download para a pasta de dados da aplicação?";
        var result = SysMessageBox.Show(Question, "Nova Versão", SysMessageBoxButtons.YesNo, SysMessageBoxIcon.Question);

        if (result == SysDialogResult.No) { return; }

        await ExecuteSystemUpdateDownloadAsync(newVersionResponse.Version, newVersionResponse.DownloadUrl, cancellationToken);
    }

    private Task<VerifyNewVersionResponse> ExecuteSystemUpdateVerificationAsync(CancellationToken cancellationToken) {
        var request = new VerifyNewVersionRequest();

        return _mediator.ExecuteAsync(request, cancellationToken);
    }

    private Task<DownloadLatestVersionResponse> ExecuteSystemUpdateDownloadAsync(string version, string downloadUrl, CancellationToken cancellationToken) {
        var request = new DownloadLatestVersionRequest(version, downloadUrl);

        return _mediator.ExecuteAsync(request, cancellationToken);
    }
}
