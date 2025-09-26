using System.Windows;

namespace Nameless.WPF.Dialogs.Message;

public static class MessageDialogExtensions {
    public static MessageDialogResult ShowInformation(this IMessageDialog self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageDialogButtons.Ok, MessageDialogIcon.Information, owner);
    }

    public static MessageDialogResult ShowError(this IMessageDialog self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageDialogButtons.Ok, MessageDialogIcon.Error, owner);
    }

    public static MessageDialogResult ShowWarning(this IMessageDialog self, string message, string? title = null, MessageDialogButtons buttons = MessageDialogButtons.Ok, Window? owner = null) {
        return self.Show(message, title, buttons, MessageDialogIcon.Warning, owner);
    }

    public static MessageDialogResult ShowAttention(this IMessageDialog self, string message, string? title = null, Window? owner = null) {
        return self.Show(message, title, MessageDialogButtons.Ok, MessageDialogIcon.Attention, owner);
    }

    public static MessageDialogResult ShowQuestion(this IMessageDialog self, string message, string? title = null, MessageDialogButtons buttons = MessageDialogButtons.YesNo, Window? owner = null) {
        return self.Show(message, title, buttons, MessageDialogIcon.Question, owner);
    }

    private static MessageDialogResult Show(this IMessageDialog self, string message, string? title, MessageDialogButtons buttons, MessageDialogIcon icon, Window? owner) {
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