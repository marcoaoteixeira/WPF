using Nameless.WPF.Notifications;
using Wpf.Ui.Controls;

namespace Nameless.WPF.UI.Notifications;

public static class NotificationTypeExtensions {
    public static ControlAppearance ToControlAppearance(this NotificationType self) {
        return self switch {
            NotificationType.Error => ControlAppearance.Danger,
            NotificationType.Success => ControlAppearance.Success,
            NotificationType.Warning => ControlAppearance.Caution,
            _ => ControlAppearance.Primary,
        };
    }
}
