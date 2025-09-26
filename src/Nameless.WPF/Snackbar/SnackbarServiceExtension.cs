using Wpf.Ui;

namespace Nameless.WPF.Snackbar;

public static class SnackbarServiceExtension {
    private static readonly TimeSpan SnackbarTimeout = TimeSpan.FromSeconds(5);

    public static void Show(this ISnackbarService self, SnackbarParameters parameters) {
        Guard.Against.Null(self);

        self.Show(
            parameters.Title ?? parameters.Appearance.GetTitle(),
            parameters.Content,
            parameters.Appearance,
            parameters.Appearance.GetIcon(),
            SnackbarTimeout
        );
    }
}