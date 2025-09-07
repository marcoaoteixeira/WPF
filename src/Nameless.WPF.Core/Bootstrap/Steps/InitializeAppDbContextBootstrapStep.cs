using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.WPF.Data;
using Nameless.WPF.Internals;
using Nameless.WPF.Resources;

namespace Nameless.WPF.Bootstrap.Steps;

public class InitializeAppDbContextBootstrapStep : BootstrapStep {
    private readonly IServiceProvider _provider;

    /// <inheritdoc />
    public override string Name => Strings.InitializeAppDbContextBootstrapStep_Name;

    /// <inheritdoc />
    public override int Order => 0;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeAppDbContextBootstrapStep"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public InitializeAppDbContextBootstrapStep(IServiceProvider provider, ILogger<InitializeAppDbContextBootstrapStep> logger)
        : base(logger) { _provider = Guard.Against.Null(provider); }

    /// <inheritdoc />
    protected override async Task InnerExecuteAsync(CancellationToken cancellationToken) {
        using var scope = _provider.CreateScope();

        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetLogger<InitializeAppDbContextBootstrapStep>();

        if (!appDbContext.Database.IsRelational()) {
            logger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        await appDbContext.Database.MigrateAsync(cancellationToken);
    }
}
