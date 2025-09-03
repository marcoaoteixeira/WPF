using System.Windows;

namespace Nameless.WPF.UI.Dialogs.DirectorySelectionBox;

public record DirectorySelectionOptions {
    public string? Title { get; init; }
    public string? Root { get; init; }
    public bool Multiselect { get; init; }
    public Window? Owner { get; init; }
}