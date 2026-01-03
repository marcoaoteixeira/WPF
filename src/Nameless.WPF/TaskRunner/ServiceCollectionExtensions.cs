using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nameless.WPF.TaskRunner;

public static class ServiceCollectionExtensions {
    extension(IServiceCollection self) {
        public IServiceCollection RegisterTaskRunner() {
            self.TryAddSingleton<ITaskRunner, TaskRunnerImpl>();

            return self;
        }
    }
}
