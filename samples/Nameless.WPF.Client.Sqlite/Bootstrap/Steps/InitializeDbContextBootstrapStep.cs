using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.Bootstrap;
using Nameless.WPF.Client.Sqlite.Data;
using Nameless.WPF.Client.Sqlite.Internals;
using Nameless.WPF.Client.Sqlite.Resources;

namespace Nameless.WPF.Client.Sqlite.Bootstrap.Steps;

/// <summary>
///     Step to initialize the DbContext.
/// </summary>
public class InitializeDbContextBootstrapStep : BootstrapStep {
    private readonly IServiceProvider _provider;

    /// <inheritdoc />
    public override string Name => Strings.InitializeDbContextBootstrapStep_Name;

    /// <inheritdoc />
    public override bool ThrowsOnFailure => true;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeDbContextBootstrapStep"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    public InitializeDbContextBootstrapStep(IServiceProvider provider) {
        _provider = Guard.Against.Null(provider);
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(CancellationToken cancellationToken) {
        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetLogger<InitializeDbContextBootstrapStep>();

        if (!dbContext.Database.IsRelational()) {
            logger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
