namespace Nameless.WPF.Dialogs.DirectorySelectionBox;

public interface IDirectorySelectionBox {
    IEnumerable<string> Show(Action<DirectorySelectionOptions> configure);
}