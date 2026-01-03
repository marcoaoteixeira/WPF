using Nameless.WPF.Resources;

namespace Nameless.WPF.Dialogs.Message;

public static class MessageDialogIconExtensions {
    extension(MessageDialogIcon self) {
        public SysMessageBoxImage ToSystem() {
            return self switch {
                MessageDialogIcon.Error => SysMessageBoxImage.Error,
                MessageDialogIcon.Warning => SysMessageBoxImage.Warning,
                MessageDialogIcon.Attention => SysMessageBoxImage.Exclamation,
                MessageDialogIcon.Question => SysMessageBoxImage.Question,
                _ => SysMessageBoxImage.Information,
            };
        }

        public string GetDisplayText() {
            return self switch {
                MessageDialogIcon.Error => Strings.MessageDialogIcon_Error,
                MessageDialogIcon.Warning => Strings.MessageDialogIcon_Warning,
                MessageDialogIcon.Attention => Strings.MessageDialogIcon_Attention,
                MessageDialogIcon.Question => Strings.MessageDialogIcon_Question,
                _ => Strings.MessageDialogIcon_Information,
            };
        }
    }
}