using System.Windows;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

public class MessageBoxImpl : IMessageBox {
    public MessageBoxResult Show(string message, Action<MessageBoxOptions> configure) {
        Guard.Against.NullOrWhiteSpace(message);
        Guard.Against.Null(configure);

        var options = new MessageBoxOptions();

        configure(options);

        var owner = options.Owner ??
                    Application.Current.MainWindow ??
                    throw new InvalidOperationException(Exceptions.MessageBoxImpl_OwnerIsNull);

        var result = SysMessageBox.Show(
            owner: owner,
            messageBoxText: message,
            caption: options.Title ?? options.Icon.GetDisplayText(),
            button: options.Buttons.ToSystem(),
            icon: options.Icon.ToSystem()
        );

        return result.FromSystem();
    }
}