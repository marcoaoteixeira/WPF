using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.Infrastructure;
using Nameless.WPF.DependencyInjection;

namespace Nameless.WPF.Data;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the application database context.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="assemblies">
    ///     Assemblies to scan for interceptors.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterAppDbContext(this IServiceCollection self, Assembly[] assemblies) {
        self.RegisterInterceptors(assemblies);

        self.AddDbContext<AppDbContext>((provider, builder) => {
            var applicationContext = provider.GetRequiredService<IApplicationContext>();
            var databasePath = Path.Combine(applicationContext.DataDirectoryPath, Constants.Database.DatabaseFileName);
            var connectionString = string.Format(Constants.Database.ConnStrPattern, databasePath);

            builder.UseSqlite(connectionString);

            var interceptors = provider.GetServices<IInterceptor>();
            builder.AddInterceptors(interceptors);
        });

        return self;
    }

    private static void RegisterInterceptors(this IServiceCollection self, Assembly[] assemblies) {
        var service = typeof(IInterceptor);
        var descriptors = assemblies.GetImplementations(service)
                                    .Where(type => !type.IsGenericTypeDefinition)
                                    .Select(implementation => CreateServiceDescriptor(service, implementation));

        self.TryAddEnumerable(descriptors);

        return;

        static ServiceDescriptor CreateServiceDescriptor(Type service, Type implementation) {
            var lifetime = ServiceLifetimeAttribute.GetLifetime(implementation);

            return new ServiceDescriptor(service, implementation, lifetime);
        }
    }
}
