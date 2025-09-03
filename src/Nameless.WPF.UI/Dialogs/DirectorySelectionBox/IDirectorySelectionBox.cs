namespace Nameless.WPF.UI.Dialogs.DirectorySelectionBox;

public interface IDirectorySelectionBox {
    IEnumerable<string> Show(DirectorySelectionOptions options);
}