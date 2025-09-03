using System.Windows;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

public class MessageBoxImpl : IMessageBox {
    public MessageBoxResult Show(MessageBoxOptions options) {
        Guard.Against.Null(options);

        var owner = options.Owner ??
                    Application.Current.MainWindow ??
                    throw new InvalidOperationException(Exceptions.MessageBoxImpl_OwnerIsNull);

        var result = SysMessageBox.Show(
            owner: owner,
            messageBoxText: options.Message,
            caption: options.Title ?? options.Icon.GetDisplayText(),
            button: options.Buttons.ToSystem(),
            icon: options.Icon.ToSystem()
        );

        return result.FromSystem();
    }
}