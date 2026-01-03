using Wpf.Ui;

namespace Nameless.WPF.Snackbar;

public static class SnackbarServiceExtension {
    private static readonly TimeSpan SnackbarTimeout = TimeSpan.FromSeconds(5);

    extension(ISnackbarService self) {
        public void Show(SnackbarParameters parameters) {
            self.Show(
                parameters.Title ?? parameters.Appearance.GetTitle(),
                parameters.Content,
                parameters.Appearance,
                parameters.Appearance.GetIcon(),
                SnackbarTimeout
            );
        }
    }
}