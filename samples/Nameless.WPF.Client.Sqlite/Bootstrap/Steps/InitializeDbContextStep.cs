using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nameless.Bootstrap;
using Nameless.WPF.Bootstrap;
using Nameless.WPF.Client.Sqlite.Data;
using Nameless.WPF.Client.Sqlite.Resources;

namespace Nameless.WPF.Client.Sqlite.Bootstrap.Steps;

/// <summary>
///     Step to initialize the DbContext.
/// </summary>
public class InitializeDbContextStep : StepBase {
    private readonly IServiceProvider _provider;

    /// <inheritdoc />
    public override string Name => Strings.InitializeDbContextBootstrapStep_Name;
    
    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="InitializeDbContextStep"/> class.
    /// </summary>
    /// <param name="provider">
    ///     The service provider.
    /// </param>
    public InitializeDbContextStep(IServiceProvider provider) {
        _provider = provider;
    }

    /// <inheritdoc />
    public override async Task ExecuteAsync(FlowContext context, CancellationToken cancellationToken) {
        var progress = context.GetStepProgress();

        progress.Report(new StepReport(Name, "Inicializando contexto da base de dados..."));

        await Task.Delay(250, cancellationToken);

        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetLogger<InitializeDbContextStep>();

        if (!dbContext.Database.IsRelational()) {
            progress.Report(new StepReport(Name, "Base de dados não relacional."));

            logger.SkipMigrationForNonRelationalDatabase();

            return;
        }

        progress.Report(new StepReport(Name, "Aplicando migração da base de dados..."));

        await Task.Delay(250, cancellationToken);

        await dbContext.Database.MigrateAsync(cancellationToken);

        progress.Report(new StepReport(Name, "Migração concluída com sucesso."));

        await Task.Delay(250, cancellationToken);
    }
}
