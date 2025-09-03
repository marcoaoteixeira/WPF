namespace Nameless.WPF.UI.Dialogs.MessageBox;

public static class MessageBoxExtensions {
    public static MessageBoxResult ShowInformation(this IMessageBox self, string message, string? title = null) {
        return self.Show(message, title, icon: MessageBoxIcon.Information);
    }

    public static MessageBoxResult ShowError(this IMessageBox self, string message, string? title = null) {
        return self.Show(message, title, icon: MessageBoxIcon.Error);
    }

    public static MessageBoxResult ShowWarning(this IMessageBox self, string message, string? title = null) {
        return self.Show(message, title, icon: MessageBoxIcon.Warning);
    }

    public static MessageBoxResult ShowAttention(this IMessageBox self, string message, string? title = null) {
        return self.Show(message, title, icon: MessageBoxIcon.Attention);
    }

    public static MessageBoxResult ShowQuestion(this IMessageBox self, string message, string? title = null, MessageBoxButtons buttons = MessageBoxButtons.YesNo) {
        return self.Show(message, title, buttons, MessageBoxIcon.Question);
    }

    private static MessageBoxResult Show(this IMessageBox self, string message, string? title = null, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxIcon icon = MessageBoxIcon.Information) {
        Guard.Against.Null(self);
        Guard.Against.NullOrWhiteSpace(message);

        return self.Show(new MessageBoxOptions {
            Title = title,
            Message = message,
            Buttons = buttons,
            Icon = icon
        });
    }
}