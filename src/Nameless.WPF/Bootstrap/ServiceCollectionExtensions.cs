using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nameless.WPF.DependencyInjection;

namespace Nameless.WPF.Bootstrap;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    extension(IServiceCollection self) {
        /// <summary>
        ///     Registers the bootstrap service and its steps.
        /// </summary>
        /// <param name="configure">
        ///     The configuration action.
        /// </param>
        /// <returns>
        ///     The current <see cref="IServiceCollection"/> so other actions
        ///     can be chained.
        /// </returns>
        public IServiceCollection RegisterBootstrapper(Action<BootstrapperOptions>? configure = null) {
            var innerConfigure = configure ?? (_ => { });
            var options = new BootstrapperOptions();

            innerConfigure(options);

            var service = typeof(Step);
            var descriptors = options.Steps
                                     .Where(type => !type.IsGenericTypeDefinition)
                                     .Select(type => type.CreateServiceDescriptor(service));

            self.TryAddEnumerable(descriptors);
            self.TryAddTransient<IBootstrapper, Bootstrapper>();

            return self;
        }
    }
}
