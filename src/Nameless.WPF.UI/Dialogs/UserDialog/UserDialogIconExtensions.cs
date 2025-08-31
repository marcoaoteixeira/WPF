using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     <see cref="UserDialogIcon"/> extensions methods.
/// </summary>
public static class UserDialogIconExtensions {
    /// <summary>
    ///     Retrieves the <see cref="SymbolRegular"/> that represents
    ///     the current <see cref="UserDialogIcon"/>.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="UserDialogIcon"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="SymbolRegular"/> that represents the
    ///     <see cref="UserDialogIcon"/>.
    /// </returns>
    public static SymbolRegular ToSymbolRegular(this UserDialogIcon self) {
        return self switch {
            UserDialogIcon.Information => SymbolRegular.ChatSparkle48,
            UserDialogIcon.Error => SymbolRegular.ChatDismiss24,
            UserDialogIcon.Warning => SymbolRegular.ChatWarning24,
            UserDialogIcon.Attention => SymbolRegular.HandLeftChat28,
            UserDialogIcon.Question => SymbolRegular.ChatHelp24,
            _ => SymbolRegular.Chat32
        };
    }

    /// <summary>
    ///     Retrieves the brush that represents
    ///     the current <see cref="UserDialogIcon"/>.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="UserDialogIcon"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="Brush"/> that represents the
    ///     <see cref="UserDialogIcon"/>.
    /// </returns>
    public static Brush ToBrush(this UserDialogIcon self) {
        return self switch {
            UserDialogIcon.Information => Brushes.CornflowerBlue,
            UserDialogIcon.Error => Brushes.Red,
            UserDialogIcon.Warning => Brushes.Orange,
            UserDialogIcon.Attention => Brushes.Gold,
            UserDialogIcon.Question => Brushes.CornflowerBlue,
            _ => Brushes.WhiteSmoke
        };
    }
}
