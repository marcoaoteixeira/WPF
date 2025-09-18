using System.Windows;

namespace Nameless.WPF.Dialogs.MessageBox;

public static class MessageBoxExtensions {
    public static MessageBoxResult ShowInformation(this IMessageBox self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageBoxButtons.Ok, MessageBoxIcon.Information, owner);
    }

    public static MessageBoxResult ShowError(this IMessageBox self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageBoxButtons.Ok, MessageBoxIcon.Error, owner);
    }

    public static MessageBoxResult ShowWarning(this IMessageBox self, string message, string? title = null, MessageBoxButtons buttons = MessageBoxButtons.Ok, Window? owner = null) {
        return self.Show(message, title, buttons, MessageBoxIcon.Warning, owner);
    }

    public static MessageBoxResult ShowAttention(this IMessageBox self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageBoxButtons.Ok, MessageBoxIcon.Attention, owner);
    }

    public static MessageBoxResult ShowQuestion(this IMessageBox self, string message, string? title = null, MessageBoxButtons buttons = MessageBoxButtons.YesNo, Window? owner = null) {
        return self.Show(message, title, buttons, MessageBoxIcon.Question, owner);
    }

    private static MessageBoxResult Show(this IMessageBox self, string message, string? title, MessageBoxButtons buttons, MessageBoxIcon icon, Window? owner) {
        Guard.Against.Null(self);
        Guard.Against.NullOrWhiteSpace(message);

        return self.Show(message, opts => {
            opts.Title = title;
            opts.Buttons = buttons;
            opts.Icon = icon;
            opts.Owner = owner;
        });
    }
}