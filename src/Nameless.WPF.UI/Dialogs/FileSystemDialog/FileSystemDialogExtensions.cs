using System.Windows;

namespace Nameless.WPF.UI.Dialogs.FileSystemDialog;

public static class FileSystemDialogExtensions {
    public static IEnumerable<string> PickDirectory(this IFileSystemDialog self, Environment.SpecialFolder root = Environment.SpecialFolder.MyDocuments, string? description = null, bool multiselect = false, bool showCreateNewDirectoryButton = false, Window? owner = null) {
        return Guard.Against.Null(self).PickDirectory(root, description, multiselect, showCreateNewDirectoryButton, owner);
    }
}