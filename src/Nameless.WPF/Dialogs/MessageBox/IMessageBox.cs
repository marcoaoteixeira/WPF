namespace Nameless.WPF.Dialogs.MessageBox;

public interface IMessageBox {
    MessageBoxResult Show(string message, Action<MessageBoxOptions> configure);
}