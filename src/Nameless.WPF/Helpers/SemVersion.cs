using Nameless.WPF.Internals;

namespace Nameless.WPF.Helpers;

public static class VersionHelper {
    private static readonly Version Prime = new(1, 0, 0);

    public static Version Parse(string value) {
        Guard.Against.NullOrWhiteSpace(value);

        var matches = RegexCache.SemVersionPattern().Matches(value);
        if (matches.Count == 0) {
            return Prime;
        }

        var version = matches[0].Groups;

        var major = int.Parse(version[1].Value);
        var minor = int.Parse(version[2].Value);
        var build = int.Parse(version[3].Value);

        _ = int.TryParse(version[5].Value, out var revision);

        return new Version(major, minor, build, revision);
    }
}