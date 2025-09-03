using System.Windows;
using Nameless.Infrastructure;
using Nameless.WPF.Resources;

namespace Nameless.WPF.UI.Dialogs.DirectorySelectionBox;

public class DirectorySelectionBoxImpl : IDirectorySelectionBox {
    private readonly IApplicationContext _applicationContext;

    public DirectorySelectionBoxImpl(IApplicationContext applicationContext) {
        _applicationContext = Guard.Against.Null(applicationContext);
    }

    public IEnumerable<string> Show(DirectorySelectionOptions options) {
        var fallbackTitle = options.Multiselect
            ? Strings.DirectorySelectionBoxImpl_FallbackTitle_Plural
            : Strings.DirectorySelectionBoxImpl_FallbackTitle;

        var dialog = new Microsoft.Win32.OpenFolderDialog {
            DefaultDirectory = options.Root ?? _applicationContext.DataDirectoryPath,
            Title = options.Title ?? fallbackTitle,
            Multiselect = options.Multiselect,
            ValidateNames = true
        };

        dialog.ShowDialog(options.Owner ?? Application.Current.MainWindow);

        return dialog.FolderNames;
    }
}