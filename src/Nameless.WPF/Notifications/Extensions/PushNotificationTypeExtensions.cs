using Wpf.Ui.Controls;

namespace Nameless.WPF.Notifications;

public static class PushNotificationTypeExtensions {
    extension(PushNotificationType self) {
        public ControlAppearance ToControlAppearance() {
            return self switch {
                PushNotificationType.Error => ControlAppearance.Danger,
                PushNotificationType.Success => ControlAppearance.Success,
                PushNotificationType.Warning => ControlAppearance.Caution,
                _ => ControlAppearance.Primary,
            };
        }
    }
}
