using Nameless.WPF.Bootstrap;

namespace Nameless.WPF.Client.Bootstrap;

public class FirstFakeStep : Step {
    public override string Name => "First Fake Step";

    public override async Task ExecuteAsync(CancellationToken cancellationToken) {
        Progress.Report(new StepProgressReport(Name, "Initializing first fake step..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting first 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting second 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting third 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "First fake step finished."));

        await Task.Delay(500, cancellationToken);
    }
}

public class SecondFakeStep : Step {
    public override string Name => "Second Fake Step";

    public override async Task ExecuteAsync(CancellationToken cancellationToken) {
        Progress.Report(new StepProgressReport(Name, "Initializing second fake step..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting first 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting second 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Waiting third 500ms..."));

        await Task.Delay(500, cancellationToken);

        Progress.Report(new StepProgressReport(Name, "Second fake step finished."));

        await Task.Delay(500, cancellationToken);
    }
}