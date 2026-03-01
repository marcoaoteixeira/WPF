using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.Helpers;
using Nameless.Infrastructure;
using Nameless.IO.FileSystem;

namespace Nameless.WPF.EntityFrameworkCore;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterEntityFrameworkCore<TDbContext>(Action<EntityFrameworkCoreRegistrationSettings> registration, Action<EntityFrameworkCoreOptions>? configure = null)
            where TDbContext : DbContext {
            self.Configure(configure ?? (_ => { }));

            return self.InnerRegisterEntityFrameworkCore<TDbContext>(registration);
        }
        
        public IServiceCollection RegisterEntityFrameworkCore<TDbContext>(Action<EntityFrameworkCoreRegistrationSettings> registration, IConfiguration configuration)
            where TDbContext : DbContext {
            self.Configure<EntityFrameworkCoreOptions>(configuration);

            return self.InnerRegisterEntityFrameworkCore<TDbContext>(registration);
        }

        private IServiceCollection InnerRegisterEntityFrameworkCore<TDbContext>(Action<EntityFrameworkCoreRegistrationSettings> registration)
            where TDbContext : DbContext {
            var settings = ActionHelper.FromDelegate(registration);

            // register interceptors, they might need injection.
            self.TryAddEnumerable(
                descriptors: CreateInterceptorDescriptors(settings)
            );

            // register database seeder
            self.TryAdd(
                descriptor: CreateDatabaseSeederDescriptor(settings)
            );
            
            self.AddDbContext<TDbContext>((provider, builder) => {
                var options = provider.GetOptions<EntityFrameworkCoreOptions>().Value;
                var fileSystem = provider.GetRequiredService<IFileSystem>();
                var databaseFilePath = fileSystem.GetFullPath(Path.Combine(
                    Constants.FolderStructure.DatabasesDirectoryName,
                    Constants.Sqlite.FileName
                ));

                var connStr = string.Format(Constants.Sqlite.ConnStrPattern, databaseFilePath);

                builder = options.Connector switch {
                    EntityFrameworkCoreConnector.Sqlite => builder.UseSqlite(connStr),
                    _ => throw new InvalidOperationException($"Missing implementation for connector '{options.Connector}'")
                };

                var interceptors = provider.GetServices<IInterceptor>();
                builder.AddInterceptors(interceptors);

                var databaseSeeder = provider.GetRequiredService<IDatabaseSeeder>();
                builder.UseAsyncSeeding(databaseSeeder.ExecuteAsync)
                       .UseSeeding(databaseSeeder.Execute);

            });

            return self;
        }
    }

    private static IEnumerable<ServiceDescriptor> CreateInterceptorDescriptors(EntityFrameworkCoreRegistrationSettings settings) {
        var service = typeof(IInterceptor);

        return settings.Interceptors.Select(implementation
            => ServiceDescriptor.Transient(
                service,
                implementation
            )
        );
    }

    private static ServiceDescriptor CreateDatabaseSeederDescriptor(EntityFrameworkCoreRegistrationSettings settings) {
        var service = typeof(IDatabaseSeeder);
        var implementation = settings.DatabaseSeeder;

        return implementation is not null
            ? ServiceDescriptor.Transient(service, implementation)
            : ServiceDescriptor.Singleton(NullDatabaseSeeder.Instance);
    }
}
