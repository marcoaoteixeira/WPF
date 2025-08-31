using Nameless.WPF.Notifications;
using Nameless.WPF.UI.Snackbar;

namespace Nameless.WPF.UI.Notifications;

public static class NotificationExtensions {
    public static SnackbarParameters ToSnackbarParameters(this INotification self) {
        return new SnackbarParameters {
            Title = self.Title,
            Content = self.Message,
            Appearance = self.Type.ToControlAppearance()
        };
    }
}
