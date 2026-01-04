using System.Text.RegularExpressions;

namespace Nameless.WPF.Internals;

internal static partial class RegexCache {
    [GeneratedRegex(@"(\d+)\.(\d+)\.(\d+)\.?(\d+)?", RegexOptions.IgnoreCase)]
    internal static partial Regex SemVersionPattern();
}
