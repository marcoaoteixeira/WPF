using Nameless.Results;

namespace Nameless.WPF;

public static class ResultExtensions {
    extension<T>(Result<T> self) {
        public bool Failure => !self.Success;
    }
}
