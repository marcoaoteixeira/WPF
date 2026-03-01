using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.Notifications.Impl;

namespace Nameless.WPF.Notifications;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterPushNotification() {
            self.TryAddSingleton<IPushNotification>(
                new PushNotification(new WeakReferenceMessenger())
            );

            return self;
        }
    }
}
