using Nameless.WPF.Resources;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

public static class MessageBoxIconExtensions {
    public static SysMessageBoxImage ToSystem(this MessageBoxIcon self) {
        return self switch {
            MessageBoxIcon.Error => SysMessageBoxImage.Error,
            MessageBoxIcon.Warning => SysMessageBoxImage.Warning,
            MessageBoxIcon.Attention => SysMessageBoxImage.Exclamation,
            MessageBoxIcon.Question => SysMessageBoxImage.Question,
            _ => SysMessageBoxImage.Information,
        };
    }

    public static string GetDisplayText(this MessageBoxIcon self) {
        return self switch {
            MessageBoxIcon.Error => Strings.MessageBoxIcon_Error,
            MessageBoxIcon.Warning => Strings.MessageBoxIcon_Warning,
            MessageBoxIcon.Attention => Strings.MessageBoxIcon_Attention,
            MessageBoxIcon.Question => Strings.MessageBoxIcon_Question,
            _ => Strings.MessageBoxIcon_Information,
        };
    }
}