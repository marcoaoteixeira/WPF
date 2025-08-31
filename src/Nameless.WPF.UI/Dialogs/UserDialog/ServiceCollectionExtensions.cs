using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.UI.Dialogs.UserDialog;

/// <summary>
///     <see cref="IServiceCollection"/> extensions methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="IUserDialog"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterUserDialog(this IServiceCollection self) {
        self.TryAddSingleton<IUserDialog, UserDialogImpl>();

        return self;
    }
}
