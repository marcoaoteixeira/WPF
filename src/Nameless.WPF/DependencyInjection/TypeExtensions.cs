using Microsoft.Extensions.DependencyInjection;

namespace Nameless.WPF.DependencyInjection;

public static class TypeExtensions {
    public static ServiceDescriptor CreateServiceDescriptor(this Type self, Type? service = null) {
        Guard.Against.Null(self);

        var lifetime = ServiceLifetimeAttribute.GetLifetime(self, fallback: ServiceLifetime.Transient);

        return new ServiceDescriptor(service ?? self, self, lifetime);
    }
}
