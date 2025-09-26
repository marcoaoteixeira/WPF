using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Dialogs.Message;

/// <summary>
///     <see cref="IServiceCollection"/> extensions methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="IMessageDialog"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterMessageDialog(this IServiceCollection self) {
        Guard.Against.Null(self);

        self.TryAddSingleton<IMessageDialog, MessageDialog>();

        return self;
    }
}
