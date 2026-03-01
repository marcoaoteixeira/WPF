using Nameless.WPF.SnackBar;

namespace Nameless.WPF.Notifications;

public static class PushNotificationMessageExtensions {
    extension(PushNotificationMessage self) {
        public SnackBarParameters ToSnackBarParameters() {
            return new SnackBarParameters {
                Title = self.Title,
                Content = self.Message,
                Appearance = self.Type.ToControlAppearance()
            };
        }
    }
}
