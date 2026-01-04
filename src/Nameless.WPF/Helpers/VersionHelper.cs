using Nameless.WPF.Internals;

namespace Nameless.WPF.Helpers;

public static class VersionHelper {
    private static readonly Version Prime = new(1, 0, 0);

    public static Version Parse(string value) {
        var matches = RegexCache.SemVersionPattern().Matches(value);
        if (matches.Count == 0) {
            return Prime;
        }

        var version = matches[0].Groups;

        var major = int.Parse(version[1].Value);
        var minor = int.Parse(version[2].Value);
        var build = int.Parse(version[3].Value);

        return int.TryParse(version[4].Value, out var revision)
            ? new Version(major, minor, build, revision)
            : new Version(major, minor, build);
    }
}