using Nameless.Bootstrap;
using Nameless.WPF.Bootstrap;

namespace Nameless.WPF.Client.Bootstrap;

public class FirstFakeStep : StepBase {
    public override string Name => "First Fake Step";

    public override async Task ExecuteAsync(FlowContext context, CancellationToken cancellationToken) {
        var progress = context.GetStepProgress();

        progress.Report(new StepReport(Name, "Initializing first fake step..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting first 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting second 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting third 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "First fake step finished."));

        await Task.Delay(500, cancellationToken);
    }
}

public class SecondFakeStep : StepBase {
    public override string Name => "Second Fake Step";

    public override async Task ExecuteAsync(FlowContext context, CancellationToken cancellationToken) {
        var progress = context.GetStepProgress();

        progress.Report(new StepReport(Name, "Initializing second fake step..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting first 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting second 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Waiting third 500ms..."));

        await Task.Delay(500, cancellationToken);

        progress.Report(new StepReport(Name, "Second fake step finished."));

        await Task.Delay(500, cancellationToken);
    }
}