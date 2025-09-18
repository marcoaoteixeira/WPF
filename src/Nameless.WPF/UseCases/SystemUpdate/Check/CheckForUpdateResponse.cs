using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Check;

public record CheckForUpdateResponse {
    public int? ReleaseID { get; }

    public string? ApplicationName { get; }

    public string? Version { get; }

    [MemberNotNullWhen(returnValue: true, nameof(ReleaseID), nameof(ApplicationName), nameof(Version))]
    public bool NewVersionAvailable => ReleaseID is not null;

    public string? Error { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Error))]
    public virtual bool Succeeded => string.IsNullOrWhiteSpace(Error);

    private CheckForUpdateResponse(int? releaseID, string? applicationName, string? version, string? error) {
        ReleaseID = releaseID;
        ApplicationName = applicationName;
        Version = version;
        Error = error;
    }

    public static CheckForUpdateResponse Skip() {
        return new CheckForUpdateResponse(
            releaseID: null,
            applicationName: null,
            version: null,
            error: null
        );
    }

    public static CheckForUpdateResponse Success(int releaseID, string applicationName, string version) {
        return new CheckForUpdateResponse(
            releaseID,
            applicationName,
            version,
            error: null
        );
    }

    public static CheckForUpdateResponse Failure(string error) {
        return new CheckForUpdateResponse(
            releaseID: null,
            applicationName: null,
            version: null,
            error
        );
    }
}