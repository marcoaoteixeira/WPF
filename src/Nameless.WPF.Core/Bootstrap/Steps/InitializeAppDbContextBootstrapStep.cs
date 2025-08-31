using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Data;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Bootstrap.Steps;

public class InitializeAppDbContextBootstrapStep : BootstrapStep {
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    public override string Name => "Initialize AppDbContext Step";

    /// <inheritdoc />
    public override int Order => 0;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeAppDbContextBootstrapStep"/> class.
    /// </summary>
    public InitializeAppDbContextBootstrapStep(IServiceProvider serviceProvider, ILogger<InitializeAppDbContextBootstrapStep> logger)
        : base(logger) {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    protected override async Task InnerExecuteAsync(CancellationToken cancellationToken) {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var dbContextLogger = scope.ServiceProvider.GetLogger<AppDbContext>();

        if (!dbContext.Database.IsRelational()) {
            dbContextLogger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
