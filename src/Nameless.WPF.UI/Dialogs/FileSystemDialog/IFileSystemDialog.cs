using System.Windows;

namespace Nameless.WPF.UI.Dialogs.FileSystemDialog;

public interface IFileSystemDialog {
    IEnumerable<string> PickDirectory(Environment.SpecialFolder root, string? description, bool multiselect, bool showCreateNewDirectoryButton, Window? owner);
}