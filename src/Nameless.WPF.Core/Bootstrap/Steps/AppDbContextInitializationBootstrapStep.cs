using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Data;
using Nameless.WPF.Internals;
using Nameless.WPF.Resources;

namespace Nameless.WPF.Bootstrap.Steps;

public class AppDbContextInitializationBootstrapStep : BootstrapStep {
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    public override string Name => Strings.AppDbContextInitializationBootstrapStep_Name;

    /// <inheritdoc />
    public override int Order => 1;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="AppDbContextInitializationBootstrapStep"/> class.
    /// </summary>
    public AppDbContextInitializationBootstrapStep(IServiceProvider serviceProvider, ILogger<AppDbContextInitializationBootstrapStep> logger)
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
