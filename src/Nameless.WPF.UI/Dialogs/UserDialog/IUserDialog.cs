using System.Windows;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     Provides way to interact with the user through messages.
/// </summary>
public interface IUserDialog {
    /// <summary>
    ///     Shows a dialog in the user interface.
    /// </summary>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="buttons">
    ///     The buttons that will be shown to the user.
    /// </param>
    /// <param name="icon">
    ///     The icon of the dialog.
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    UserDialogResult Show(string title, string message, UserDialogButtons buttons, UserDialogIcon icon, Window? owner);
}