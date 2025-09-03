using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.UI.Dialogs.MessageBox;

/// <summary>
///     <see cref="IServiceCollection"/> extensions methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="IMessageBox"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterMessageBox(this IServiceCollection self) {
        self.TryAddSingleton<IMessageBox, MessageBoxImpl>();

        return self;
    }
}
