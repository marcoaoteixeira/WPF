using Nameless.WPF.SnackBar;

namespace Nameless.WPF.Notifications;

public static class NotificationExtensions {
    extension(INotification self) {
        public SnackBarParameters ToSnackbarParameters() {
            return new SnackBarParameters {
                Title = self.Title,
                Content = self.Message,
                Appearance = self.Type.ToControlAppearance()
            };
        }
    }
}
