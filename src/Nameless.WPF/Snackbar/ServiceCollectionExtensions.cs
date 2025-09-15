using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wpf.Ui;

namespace Nameless.WPF.Snackbar;

public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterSnackbarService(this IServiceCollection self) {
        self.TryAddSingleton<ISnackbarService, SnackbarService>();

        return self;
    }
}
