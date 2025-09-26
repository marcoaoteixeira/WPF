using Nameless.WPF.Resources;

namespace Nameless.WPF.Dialogs.Message;

public static class MessageDialogIconExtensions {
    public static SysMessageBoxImage ToSystem(this MessageDialogIcon self) {
        return self switch {
            MessageDialogIcon.Error => SysMessageBoxImage.Error,
            MessageDialogIcon.Warning => SysMessageBoxImage.Warning,
            MessageDialogIcon.Attention => SysMessageBoxImage.Exclamation,
            MessageDialogIcon.Question => SysMessageBoxImage.Question,
            _ => SysMessageBoxImage.Information,
        };
    }

    public static string GetDisplayText(this MessageDialogIcon self) {
        return self switch {
            MessageDialogIcon.Error => Strings.MessageDialogIcon_Error,
            MessageDialogIcon.Warning => Strings.MessageDialogIcon_Warning,
            MessageDialogIcon.Attention => Strings.MessageDialogIcon_Attention,
            MessageDialogIcon.Question => Strings.MessageDialogIcon_Question,
            _ => Strings.MessageDialogIcon_Information,
        };
    }
}