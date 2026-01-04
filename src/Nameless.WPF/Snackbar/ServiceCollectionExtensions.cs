using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wpf.Ui;

namespace Nameless.WPF.Snackbar;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterSnackbarService() {
            self.TryAddSingleton<ISnackbarService, SnackbarService>();

            return self;
        }
    }
}
