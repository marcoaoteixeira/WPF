using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nameless.WPF;

/// <summary>
///     Factory class to create a <see cref="IHost" /> instance.
/// </summary>
public sealed class HostFactory {
    private readonly string[] _args;

    private Action<IServiceCollection, IConfiguration> _configure;

    // We want to force the usage of the static Create method
    private HostFactory(string[] args) {
        _args = args;
        _configure = (_, _) => { };
    }

    /// <summary>
    ///     Creates a new instance of <see cref="HostFactory" />.
    /// </summary>
    /// <param name="args">
    ///     The command line arguments.
    /// </param>
    /// <returns>
    ///     A new instance of <see cref="HostFactory" />.
    /// </returns>
    public static HostFactory Create(params string[] args) {
        return new HostFactory(args);
    }

    /// <summary>
    ///     Configures the services for the host.
    /// </summary>
    /// <param name="configure">
    ///     The action to configure the services.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="HostFactory" /> so other
    ///     actions can be chained.
    /// </returns>
    public HostFactory ConfigureServices(Action<IServiceCollection, IConfiguration> configure) {
        _configure = Guard.Against.Null(configure);

        return this;
    }

    /// <summary>
    ///     Builds the <see cref="IHost" /> instance.
    /// </summary>
    /// <returns>
    ///     The built <see cref="IHost" /> instance.
    /// </returns>
    public IHost Build() {
        return Host.CreateDefaultBuilder(_args)
                   .ConfigureHostConfiguration(builder =>
                       builder
                           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                           .AddJsonFile(path: "AppSettings.json", optional: true, reloadOnChange: true)
                           .AddEnvironmentVariables()
                    )
                   .ConfigureServices((ctx, services) => _configure(services, ctx.Configuration))
                   .Build();
    }
}