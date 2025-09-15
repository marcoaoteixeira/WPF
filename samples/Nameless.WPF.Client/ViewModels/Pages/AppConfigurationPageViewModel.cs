using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.Infrastructure;
using Nameless.Mediator;
using Nameless.WPF.Client.Resources;
using Nameless.WPF.Client.Sqlite.UseCases.Database.Backup;
using Nameless.WPF.Client.Views.Pages;
using Nameless.WPF.Configuration;
using Nameless.WPF.Dialogs.MessageBox;
using Nameless.WPF.Helpers;
using Nameless.WPF.Mvvm;
using Nameless.WPF.TaskRunner;
using Nameless.WPF.UseCases.SystemUpdate.Check;
using Nameless.WPF.UseCases.SystemUpdate.Download;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace Nameless.WPF.Client.ViewModels.Pages;

/// <summary>
///     View model for <see cref="AppConfigurationPage"/>.
/// </summary>
public partial class AppConfigurationPageViewModel : ViewModel, INavigationAware {
    private readonly IAppConfigurationManager _appConfigurationManager;
    private readonly IApplicationContext _applicationContext;
    private readonly IMediator _mediator;
    private readonly IMessageBox _messageBox;
    private readonly ITaskRunner _taskRunner;

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
        IMessageBox messageBox,
        ITaskRunner taskRunner) {
        _appConfigurationManager = Guard.Against.Null(appConfigurationManager);
        _applicationContext = Guard.Against.Null(applicationContext);
        _mediator = Guard.Against.Null(mediator);
        _messageBox = Guard.Against.Null(messageBox);
        _taskRunner = Guard.Against.Null(taskRunner);
    }

    public Task OnNavigatedToAsync() {
        Initialize();

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync() {
        return Task.CompletedTask;
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
        return _taskRunner.CreateBuilder()
                          .SetName(Strings.AppConfigurationPageViewModel_PerformSystemUpdate_TaskRunnerWindow_Title)
                          .SubscribeFor<CheckForUpdateNotification>()
                          .SubscribeFor<DownloadUpdateNotification>()
                          .SetDelegate(ExecuteSystemUpdateAsync)
                          .RunAsync();
    }

    [RelayCommand]
    private Task OpenApplicationDataDirectoryAsync() {
        ProcessHelper.OpenDirectory(_applicationContext.DataDirectoryPath);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OpenApplicationLogFileAsync() {
        ProcessHelper.OpenTextFile(Constants.Application.LOG_FILE_NAME);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task PerformDatabaseBackupAsync() {
        return _taskRunner.CreateBuilder()
                          .SetName(Strings.AppConfigurationPageViewModel_PerformDatabaseBackup_TaskRunnerWindow_Title)
                          .SubscribeFor<PerformDatabaseBackupNotification>()
                          .SetDelegate(ExecuteDatabaseBackupAsync)
                          .RunAsync();
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

    private async Task ExecuteDatabaseBackupAsync(CancellationToken cancellationToken) {
        _ = await _mediator.ExecuteAsync(new PerformDatabaseBackupRequest(), cancellationToken)
                           .SuppressContext();
    }

    private async Task ExecuteSystemUpdateAsync(CancellationToken cancellationToken) {
        var checkForUpdateResponse = await ExecuteCheckForUpdateAsync(cancellationToken).SuppressContext();

        if (!checkForUpdateResponse.NewVersionAvailable) { return; }

        var result = _messageBox.ShowQuestion(
            title: Strings.AppConfigurationPageViewModel_ExecuteSystemUpdateAsync_ConfirmDownload_MessageBox_Title,
            message: Strings.AppConfigurationPageViewModel_ExecuteSystemUpdateAsync_ConfirmDownload_MessageBox_Message
        );

        if (result == MessageBoxResult.No) { return; }

        await ExecuteDownloadUpdateAsync(checkForUpdateResponse.Version, checkForUpdateResponse.DownloadUrl, cancellationToken);
    }

    private Task<CheckForUpdateResponse> ExecuteCheckForUpdateAsync(CancellationToken cancellationToken) {
        return _mediator.ExecuteAsync(
            new CheckForUpdateRequest(),
            cancellationToken
        );
    }

    private Task<DownloadUpdateResponse> ExecuteDownloadUpdateAsync(string version, string downloadUrl, CancellationToken cancellationToken) {
        return _mediator.ExecuteAsync(
            new DownloadUpdateRequest(version, downloadUrl),
            cancellationToken
        );
    }
}
