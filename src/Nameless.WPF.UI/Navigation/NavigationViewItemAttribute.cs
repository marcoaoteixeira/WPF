using Wpf.Ui.Controls;

namespace Nameless.WPF.UI.Navigation;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NavigationViewItemAttribute : Attribute {
    public required SymbolRegular Icon { get; init; }
    public required string Title { get; init; }
    public string? ToolTip { get; init; }
    public int Position { get; init; }
    public bool Footer { get; init; }
}
