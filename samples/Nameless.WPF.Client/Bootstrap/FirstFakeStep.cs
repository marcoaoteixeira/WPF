using Nameless.Bootstrap;
using Nameless.Bootstrap.Infrastructure;
using Nameless.Bootstrap.Notification;

namespace Nameless.WPF.Client.Bootstrap;

public class FirstFakeStep : StepBase {
    public override string Name => "First Fake Step";

    public override async Task ExecuteAsync(FlowContext context, IProgress<StepProgress> progress, CancellationToken cancellationToken) {
        progress.ReportInformation(Name, "Initializing first fake step...");

        await Task.Delay(500, cancellationToken);

        progress.ReportInformation(Name, "Waiting first 500ms...");

        await Task.Delay(500, cancellationToken);

        progress.ReportInformation(Name, "Waiting second 500ms...");

        await Task.Delay(500, cancellationToken);

        progress.ReportInformation(Name, "Waiting third 500ms...");

        await Task.Delay(500, cancellationToken);

        progress.ReportInformation(Name, "First fake step finished.");

        await Task.Delay(500, cancellationToken);
    }
}