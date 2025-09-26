using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.TaskRunner;

public static class ServiceCollectionExtensions {
    public static IServiceCollection RegisterTaskRunner(this IServiceCollection self) {
        Guard.Against.Null(self);

        self.TryAddSingleton<ITaskRunner, TaskRunnerImpl>();

        return self;
    }
}
