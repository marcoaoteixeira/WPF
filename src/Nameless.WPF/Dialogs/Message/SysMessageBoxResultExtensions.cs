namespace Nameless.WPF.Dialogs.Message;

public static class SysMessageBoxResultExtensions {
    public static MessageDialogResult FromSystem(this SysMessageBoxResult self) {
        return self switch {
            SysMessageBoxResult.Cancel => MessageDialogResult.Cancel,
            SysMessageBoxResult.Yes => MessageDialogResult.Yes,
            SysMessageBoxResult.No => MessageDialogResult.No,
            _ => MessageDialogResult.Ok,
        };
    }
}