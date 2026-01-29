using Wpf.Ui.Controls;

namespace Nameless.WPF.SnackBar;

public record SnackBarParameters {
    public string? Title { get; init; }
    public string Content { get; init; } = string.Empty;
    public ControlAppearance Appearance { get; init; }
}
