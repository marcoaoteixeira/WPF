using Wpf.Ui.Controls;

namespace Nameless.WPF.Notifications;

public static class NotificationTypeExtensions {
    extension(NotificationType self) {
        public ControlAppearance ToControlAppearance() {
            return self switch {
                NotificationType.Error => ControlAppearance.Danger,
                NotificationType.Success => ControlAppearance.Success,
                NotificationType.Warning => ControlAppearance.Caution,
                _ => ControlAppearance.Primary,
            };
        }
    }
}
