using Wpf.Ui.Controls;

namespace Nameless.WPF.Snackbar;

public sealed record SnackbarParameters {
    public string? Title { get; init; }
    public string Content { get; init; } = string.Empty;
    public ControlAppearance Appearance { get; init; }
}
