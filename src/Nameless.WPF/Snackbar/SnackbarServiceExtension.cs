using Wpf.Ui;

namespace Nameless.WPF.SnackBar;

public static class SnackBarServiceExtension {
    private static readonly TimeSpan SnackBarTimeout = TimeSpan.FromSeconds(5);

    extension(ISnackbarService self) {
        public void Show(SnackBarParameters parameters) {
            self.Show(
                parameters.Title ?? parameters.Appearance.GetTitle(),
                parameters.Content,
                parameters.Appearance,
                parameters.Appearance.GetIcon(),
                SnackBarTimeout
            );
        }
    }
}