using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Notifications;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterNotificationService() {
            self.TryAddSingleton<IMessenger, WeakReferenceMessenger>();
            self.TryAddSingleton<INotificationService, NotificationService>();

            return self;
        }
    }
}
