using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.Dialogs.FileSystem;

/// <summary>
///     <see cref="IServiceCollection"/> extensions methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Registers the <see cref="IFileSystemDialog"/> service.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterFileSystemDialog(this IServiceCollection self) {
        Guard.Against.Null(self);

        self.TryAddSingleton<IFileSystemDialog, FileSystemDialog>();

        return self;
    }
}
