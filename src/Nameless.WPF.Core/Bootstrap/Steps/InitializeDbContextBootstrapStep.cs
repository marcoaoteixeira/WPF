using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Internals;
using Nameless.WPF.Resources;

namespace Nameless.WPF.Bootstrap.Steps;

/// <summary>
///     Step to initialize the DbContext.
/// </summary>
public class InitializeDbContextBootstrapStep : BootstrapStep {
    private readonly IServiceProvider _provider;

    /// <inheritdoc />
    public override string Name => Strings.InitializeDbContextBootstrapStep_Name;

    /// <inheritdoc />
    public override int Order => 0;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeDbContextBootstrapStep"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public InitializeDbContextBootstrapStep(IServiceProvider provider, ILogger<InitializeDbContextBootstrapStep> logger)
        : base(logger) { _provider = Guard.Against.Null(provider); }

    /// <inheritdoc />
    protected override async Task InnerExecuteAsync(CancellationToken cancellationToken) {
        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var logger = scope.ServiceProvider.GetLogger<InitializeDbContextBootstrapStep>();

        if (!dbContext.Database.IsRelational()) {
            logger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
