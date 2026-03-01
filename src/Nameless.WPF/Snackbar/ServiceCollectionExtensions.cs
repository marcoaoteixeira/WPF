using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wpf.Ui;

namespace Nameless.WPF.SnackBar;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
<<<<<<< Updated upstream
        public IServiceCollection RegisterSnackBarService() {
=======
        public IServiceCollection RegisterSnackBar() {
>>>>>>> Stashed changes
            self.TryAddSingleton<ISnackbarService, SnackbarService>();

            return self;
        }
    }
}
