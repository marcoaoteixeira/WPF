using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Notifications;

public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterNotificationService(this IServiceCollection self) {
        Guard.Against.Null(self);

        self.TryAddSingleton<IMessenger, WeakReferenceMessenger>();
        self.TryAddSingleton<INotificationService, NotificationService>();

        return self;
    }
}
