using Nameless.Bootstrap;
using Nameless.Null;

namespace Nameless.WPF.Bootstrap;

public readonly record struct StepReport(string Title, string Message);

public static class FlowContextExtensions {
    private const string STEP_REPORT_PROGRESS_KEY = "IProgress<StepReport>";

    extension(FlowContext self) {
        public FlowContext SetStepProgress(IProgress<StepReport> progress) {
            self[STEP_REPORT_PROGRESS_KEY] = progress;

            return self;
        }

        public IProgress<StepReport> GetStepProgress() {
            return self[STEP_REPORT_PROGRESS_KEY] as Progress<StepReport>
                   ?? NullProgress<StepReport>.Instance;
        }
    }
}
