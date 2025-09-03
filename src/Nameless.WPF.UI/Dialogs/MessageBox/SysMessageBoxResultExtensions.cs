namespace Nameless.WPF.UI.Dialogs.MessageBox;

public static class SysMessageBoxResultExtensions {
    public static MessageBoxResult FromSystem(this SysMessageBoxResult self) {
        return self switch {
            SysMessageBoxResult.Cancel => MessageBoxResult.Cancel,
            SysMessageBoxResult.Yes => MessageBoxResult.Yes,
            SysMessageBoxResult.No => MessageBoxResult.No,
            _ => MessageBoxResult.Ok,
        };
    }
}