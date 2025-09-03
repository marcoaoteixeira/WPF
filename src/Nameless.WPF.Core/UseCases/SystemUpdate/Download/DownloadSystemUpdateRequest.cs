using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public record DownloadSystemUpdateRequest(string Version, string DownloadUrl) : IRequest<DownloadSystemUpdateResponse>;