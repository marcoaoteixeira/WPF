namespace Nameless.WPF.Dialogs.MessageBox;

public class MessageBoxImpl : IMessageBox {
    public MessageBoxResult Show(string message, Action<MessageBoxOptions> configure) {
        Guard.Against.NullOrWhiteSpace(message);
        Guard.Against.Null(configure);

        var options = new MessageBoxOptions();

        configure(options);

        SysMessageBoxResult result;
        if (options.Owner is null) {
            result = SysMessageBox.Show(
                messageBoxText: message,
                caption: options.Title ?? options.Icon.GetDisplayText(),
                button: options.Buttons.ToSystem(),
                icon: options.Icon.ToSystem()
            );
        }
        else {
            result = SysMessageBox.Show(
                owner: options.Owner,
                messageBoxText: message,
                caption: options.Title ?? options.Icon.GetDisplayText(),
                button: options.Buttons.ToSystem(),
                icon: options.Icon.ToSystem()
            );
        }

        return result.FromSystem();
    }
}