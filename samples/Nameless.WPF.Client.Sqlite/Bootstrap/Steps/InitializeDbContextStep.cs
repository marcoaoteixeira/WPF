using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nameless.WPF.Bootstrap;
using Nameless.WPF.Client.Sqlite.Data;
using Nameless.WPF.Client.Sqlite.Resources;

namespace Nameless.WPF.Client.Sqlite.Bootstrap.Steps;

/// <summary>
///     Step to initialize the DbContext.
/// </summary>
public class InitializeDbContextStep : Step {
    private readonly IServiceProvider _provider;

    /// <inheritdoc />
    public override string Name => Strings.InitializeDbContextBootstrapStep_Name;

    /// <inheritdoc />
    public override bool ThrowsOnFailure => true;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeDbContextStep"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    public InitializeDbContextStep(IServiceProvider provider) {
        _provider = Guard.Against.Null(provider);
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(CancellationToken cancellationToken) {
        Progress.Report(new StepProgressReport(Name, "Inicializando serviços..."));

        await Task.Delay(250, cancellationToken);

        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetLogger<InitializeDbContextStep>();

        if (!dbContext.Database.IsRelational()) {
            Progress.Report(new StepProgressReport(Name, "Base de dados não relacional."));

            logger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        Progress.Report(new StepProgressReport(Name, "Aplicando migração da base de dados..."));

        await Task.Delay(250, cancellationToken);

        await dbContext.Database.MigrateAsync(cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Migração concluída com sucesso."));

        await Task.Delay(250, cancellationToken);
    }
}
