namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     Represents which buttons the user should be presented.
/// </summary>
public enum UserDialogButtons {
    /// <summary>
    ///    Button for acknowledge.
    /// </summary>
    Ok,

    /// <summary>
    ///     Buttons for acknowledge or cancel.
    /// </summary>
    OkCancel,

    /// <summary>
    ///     Buttons for accept or deny.
    /// </summary>
    YesNo,

    /// <summary>
    ///     Buttons for accept, deny or cancel.
    /// </summary>
    YesNoCancel,
}
