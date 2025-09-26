namespace Nameless.WPF.Dialogs.Message;

public static class MessageDialogButtonsExtensions {
    public static SysMessageBoxButton ToSystem(this MessageDialogButtons self) {
        return self switch {
            MessageDialogButtons.OkCancel => SysMessageBoxButton.OKCancel,
            MessageDialogButtons.YesNo => SysMessageBoxButton.YesNo,
            MessageDialogButtons.YesNoCancel => SysMessageBoxButton.YesNoCancel,
            _ => SysMessageBoxButton.OK,
        };
    }
}