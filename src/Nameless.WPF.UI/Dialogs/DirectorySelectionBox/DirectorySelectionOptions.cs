using System.Windows;

namespace Nameless.WPF.UI.Dialogs.DirectorySelectionBox;

public class DirectorySelectionOptions {
    public string? Title { get; set; }
    public string? Root { get; set; }
    public bool Multiselect { get; set; }
    public Window? Owner { get; set; }
}