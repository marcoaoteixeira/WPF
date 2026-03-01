using Nameless.Bootstrap;
using Nameless.Bootstrap.Infrastructure;
using Nameless.Bootstrap.Notification;
using Nameless.IO.FileSystem;

namespace Nameless.WPF.Bootstrap;

public class EnsureApplicationDirectoriesExistenceStep : StepBase {
    private readonly IFileSystem _fileSystem;

    public override string Name => "Ensure Application Directories Existence";

    public EnsureApplicationDirectoriesExistenceStep(IFileSystem fileSystem) {
        _fileSystem = fileSystem;
    }

    public override Task ExecuteAsync(FlowContext context, IProgress<StepProgress> progress, CancellationToken cancellationToken) {
        progress.ReportInformation(Name, "Creating backup directory...");
        _fileSystem.EnsureBackupsDirectoryExistence();

        progress.ReportInformation(Name, "Creating database directory...");
        _fileSystem.EnsureDatabaseDirectoryExistence();

        progress.ReportInformation(Name, "Creating temporary directory...");
        _fileSystem.EnsureTemporaryDirectoryExistence();

        progress.ReportInformation(Name, "Creating updates directory...");
        _fileSystem.EnsureUpdatesDirectoryExistence();

        return Task.CompletedTask;
    }
}
