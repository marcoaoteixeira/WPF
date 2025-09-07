using Microsoft.EntityFrameworkCore.Diagnostics;
using Nameless.WPF.Data;

namespace Nameless.WPF.Testing.Tools;

public class AppDbContextFactoryOptions {
    internal HashSet<IInterceptor> Interceptors { get; } = [];

    internal Action<AppDbContext, bool>? Seeding { get; set; }

    internal Func<AppDbContext, bool, CancellationToken, Task>? SeedingAsync { get; set; }

    public bool UseInMemory { get; set; }

    public AppDbContextFactoryOptions UseInterceptor(IInterceptor interceptor) {
        Interceptors.Add(interceptor);

        return this;
    }

    public AppDbContextFactoryOptions UseSeeding(Action<AppDbContext, bool> seeding) {
        Seeding = seeding;

        return this;
    }

    public AppDbContextFactoryOptions UseSeeding(Func<AppDbContext, bool, CancellationToken, Task> seeding) {
        SeedingAsync = seeding;

        return this;
    }
}