using System.Windows;
using Nameless.WPF.UI.Windows;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     Defines the behavior of a user dialog window.
/// </summary>
public interface IUserDialogWindow : IWindow {
    /// <summary>
    ///     Sets the dialog title.
    /// </summary>
    /// <param name="title">
    ///     The dialog title.
    /// </param>
    /// <returns>
    ///     The current <see cref="IUserDialogWindow"/> so other actions
    ///     can be chained.
    /// </returns>
    IUserDialogWindow SetTitle(string title);

    /// <summary>
    ///     Sets the dialog message.
    /// </summary>
    /// <param name="message">
    ///     The dialog message.
    /// </param>
    /// <returns>
    ///     The current <see cref="IUserDialogWindow"/> so other actions
    ///     can be chained.
    /// </returns>
    IUserDialogWindow SetMessage(string message);

    /// <summary>
    ///     Sets the dialog buttons.
    /// </summary>
    /// <param name="buttons">
    ///     The dialog buttons.
    /// </param>
    /// <returns>
    ///     The current <see cref="IUserDialogWindow"/> so other actions
    ///     can be chained.
    /// </returns>
    IUserDialogWindow SetButtons(UserDialogButtons buttons);

    /// <summary>
    ///     Sets the dialog icon.
    /// </summary>
    /// <param name="icon">
    ///     The dialog icon.
    /// </param>
    /// <returns>
    ///     The current <see cref="IUserDialogWindow"/> so other actions
    ///     can be chained.
    /// </returns>
    IUserDialogWindow SetIcon(UserDialogIcon icon);

    /// <summary>
    ///     Sets the dialog owner.
    /// </summary>
    /// <param name="owner">
    ///     The dialog owner.
    /// </param>
    /// <returns>
    ///     The current <see cref="IUserDialogWindow"/> so other actions
    ///     can be chained.
    /// </returns>
    IUserDialogWindow SetOwner(Window? owner);

    /// <summary>
    ///     Shows the dialog and returns the user choice.
    /// </summary>
    /// <param name="startupLocation">
    ///     The window startup location in the screen.
    /// </param>
    /// <returns>
    ///     The user choice.
    /// </returns>
    UserDialogResult ShowDialog(WindowStartupLocation startupLocation);
}