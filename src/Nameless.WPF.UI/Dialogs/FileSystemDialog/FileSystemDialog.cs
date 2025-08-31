using System.Windows;
using Ookii.Dialogs.Wpf;

namespace Nameless.WPF.UI.Dialogs.FileSystemDialog;

public class FileSystemDialog : IFileSystemDialog {
    public IEnumerable<string> PickDirectory(Environment.SpecialFolder root, string? description, bool multiselect, bool showCreateNewDirectoryButton, Window? owner) {
        var dialog = new VistaFolderBrowserDialog {
            RootFolder = root,
            Multiselect = multiselect,
            ShowNewFolderButton = showCreateNewDirectoryButton
        };

        if (!string.IsNullOrWhiteSpace(description)) {
            dialog.Description = description;
            dialog.UseDescriptionForTitle = true;
        }

        var result = dialog.ShowDialog(owner ?? Application.Current.MainWindow);
        if (result.GetValueOrDefault()) {
            return multiselect ? dialog.SelectedPaths : [dialog.SelectedPath];
        }

        return [];
    }
}