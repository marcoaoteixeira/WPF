using Nameless.WPF.Snackbar;

namespace Nameless.WPF.Notifications;

public static class NotificationExtensions {
    extension(INotification self) {
        public SnackbarParameters ToSnackbarParameters() {
            return new SnackbarParameters {
                Title = self.Title,
                Content = self.Message,
                Appearance = self.Type.ToControlAppearance()
            };
        }
    }
}
