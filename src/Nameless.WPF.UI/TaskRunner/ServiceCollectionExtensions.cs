using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.UI.TaskRunner;

public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterTaskRunner(this IServiceCollection self) {
        self.TryAddSingleton<ITaskRunner, TaskRunnerImpl>();

        return self;
    }
}
