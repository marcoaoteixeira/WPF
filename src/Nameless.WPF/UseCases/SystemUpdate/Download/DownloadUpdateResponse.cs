using Nameless.ObjectModel;
using Nameless.Results;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public class DownloadUpdateResponse : Result<DownloadedUpdateMetadata> {
    private DownloadUpdateResponse(DownloadedUpdateMetadata value, Error[] errors)
        : base(value, errors) { }

    public static implicit operator DownloadUpdateResponse(DownloadedUpdateMetadata value) {
        return new DownloadUpdateResponse(value, errors: []);
    }

    public static implicit operator DownloadUpdateResponse(Error error) {
        return new DownloadUpdateResponse(value: default, errors: [error]);
    }
}