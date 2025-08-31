using System.Windows;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     <see cref="IUserDialog"/> extensions methods.
/// </summary>
public static class UserDialogExtensions {
    /// <summary>
    ///     Shows an information dialog with OK button.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IUserDialog"/>.
    /// </param>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    public static UserDialogResult ShowInformation(this IUserDialog self, string title, string message, Window? owner = null) {
        return self.Show(title, message, UserDialogButtons.Ok, UserDialogIcon.Information, owner);
    }

    /// <summary>
    ///     Shows an error dialog.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IUserDialog"/>.
    /// </param>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="buttons">
    ///     The buttons that will be shown to the user
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    public static UserDialogResult ShowError(this IUserDialog self, string title, string message, UserDialogButtons buttons = UserDialogButtons.Ok, Window? owner = null) {
        return self.Show(title, message, buttons, UserDialogIcon.Error, owner);
    }

    /// <summary>
    ///     Shows a warning.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IUserDialog"/>.
    /// </param>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="buttons">
    ///     The buttons that will be shown to the user
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    public static UserDialogResult ShowWarning(this IUserDialog self, string title, string message, UserDialogButtons buttons = UserDialogButtons.Ok, Window? owner = null) {
        return self.Show(title, message, buttons, UserDialogIcon.Warning, owner);
    }

    /// <summary>
    ///     Shows an attention dialog.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IUserDialog"/>.
    /// </param>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="buttons">
    ///     The buttons that will be shown to the user
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    public static UserDialogResult ShowAttention(this IUserDialog self, string title, string message, UserDialogButtons buttons = UserDialogButtons.Ok, Window? owner = null) {
        return self.Show(title, message, buttons, UserDialogIcon.Attention, owner);
    }

    /// <summary>
    ///     Shows a question dialog.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IUserDialog"/>.
    /// </param>
    /// <param name="title">
    ///     The title of the dialog.
    /// </param>
    /// <param name="message">
    ///     The message of the dialog.
    /// </param>
    /// <param name="buttons">
    ///     The buttons that will be shown to the user
    /// </param>
    /// <param name="owner">
    ///     The <see cref="Window"/> that it is its owner.
    /// </param>
    /// <returns>
    ///     A result from the user interaction.
    /// </returns>
    public static UserDialogResult ShowQuestion(this IUserDialog self, string title, string message, UserDialogButtons buttons, Window? owner = null) {
        return self.Show(title, message, buttons, UserDialogIcon.Question, owner);
    }
}
