using System.Windows;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     Default implementation of <see cref="IUserDialog"/>.
/// </summary>
public class UserDialogImpl : IUserDialog {
    private readonly IWindowFactory _factory;

    /// <summary>
    ///     Initializes a new instance
    ///     of <see cref="UserDialogImpl"/> class.
    /// </summary>
    /// <param name="factory">
    ///     The window factory.
    /// </param>
    public UserDialogImpl(IWindowFactory factory) {
        _factory = Guard.Against.Null(factory);
    }

    /// <inheritdoc />
    public UserDialogResult Show(string title, string message, UserDialogButtons buttons, UserDialogIcon icon, Window? owner) {
        if (_factory.TryCreate<IUserDialogWindow>(out var window)) {
            return window.SetTitle(title)
                         .SetMessage(message)
                         .SetButtons(buttons)
                         .SetIcon(icon)
                         .SetOwner(owner)
                         .ShowDialog(WindowStartupLocation.CenterScreen);
        }

        throw new InvalidOperationException("Unable to create user dialog window.");
    }
}