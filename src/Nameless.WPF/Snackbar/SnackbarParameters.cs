using Wpf.Ui.Controls;

namespace Nameless.WPF.SnackBar;

public record SnackBarParameters {
<<<<<<< Updated upstream
=======
    public required string Content { get; init; }
>>>>>>> Stashed changes
    public string? Title { get; init; }
    public ControlAppearance Appearance { get; init; }
}
