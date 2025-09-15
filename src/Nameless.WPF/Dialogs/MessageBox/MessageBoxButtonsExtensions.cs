namespace Nameless.WPF.Dialogs.MessageBox;

public static class MessageBoxButtonsExtensions {
    public static SysMessageBoxButton ToSystem(this MessageBoxButtons self) {
        return self switch {
            MessageBoxButtons.OkCancel => SysMessageBoxButton.OKCancel,
            MessageBoxButtons.YesNo => SysMessageBoxButton.YesNo,
            MessageBoxButtons.YesNoCancel => SysMessageBoxButton.YesNoCancel,
            _ => SysMessageBoxButton.OK,
        };
    }
}