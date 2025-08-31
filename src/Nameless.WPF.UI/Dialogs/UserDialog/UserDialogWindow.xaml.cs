using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.DependencyInjection;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     A window that creates a dialog for the user.
/// </summary>
[ServiceLifetime(Lifetime = ServiceLifetime.Transient)]
public partial class UserDialogWindow : IUserDialogWindow {
    private UserDialogResult _result = UserDialogResult.Ok;

    /// <summary>
    ///     Initializes a new instance of <see cref="UserDialogWindow"/>.
    /// </summary>
    public UserDialogWindow() {
        InitializeComponent();
    }

    public IUserDialogWindow SetTitle(string title) {
        CallOnDispatcher(() => TitleTextBlock.Text = title);

        return this;
    }

    public IUserDialogWindow SetMessage(string message) {
        CallOnDispatcher(() => MessageTextBlock.Text = message);

        return this;
    }

    public IUserDialogWindow SetButtons(UserDialogButtons buttons) {
        CallOnDispatcher(() => {
            OkButton.Visibility = buttons is UserDialogButtons.Ok or UserDialogButtons.OkCancel
                ? Visibility.Visible
                : Visibility.Collapsed;

            YesButton.Visibility = buttons is UserDialogButtons.YesNo or UserDialogButtons.YesNoCancel
                ? Visibility.Visible
                : Visibility.Collapsed;

            NoButton.Visibility = buttons is UserDialogButtons.YesNo or UserDialogButtons.YesNoCancel
                ? Visibility.Visible
                : Visibility.Collapsed;

            CancelButton.Visibility = buttons is UserDialogButtons.OkCancel or UserDialogButtons.YesNoCancel
                ? Visibility.Visible
                : Visibility.Collapsed;
        });

        return this;
    }

    public IUserDialogWindow SetIcon(UserDialogIcon icon) {
        CallOnDispatcher(() => {
            IconSymbolIcon.Symbol = icon.ToSymbolRegular();
            IconSymbolIcon.Foreground = icon.ToBrush();
        });

        return this;
    }

    public IUserDialogWindow SetOwner(Window? owner) {
        CallOnDispatcher(() => Owner = owner ?? Application.Current.MainWindow);

        return this;
    }

    public UserDialogResult ShowDialog(WindowStartupLocation startupLocation) {
        WindowStartupLocation = startupLocation;

        CallOnDispatcher(() => ShowDialog());

        return _result;
    }

    void IWindow.Show(WindowStartupLocation startupLocation) {
        WindowStartupLocation = startupLocation;

        CallOnDispatcher(Show);
    }

    private void ButtonHandler(object sender, RoutedEventArgs _) {
        if (sender is Button { Tag: UserDialogResult result }) {
            DialogResult = result switch {
                UserDialogResult.Yes or
                UserDialogResult.Ok => true,
                _ => false
            };

            _result = result;
        }

        Close();
    }

    private void CallOnDispatcher(Action action) {
        var dispatch = Dispatcher.CheckAccess();

        if (dispatch) { Dispatcher.Invoke(action); }
        else { action(); }
    }
}