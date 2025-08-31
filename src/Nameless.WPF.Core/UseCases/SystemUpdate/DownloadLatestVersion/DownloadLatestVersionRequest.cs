using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.SystemUpdate.DownloadLatestVersion;

public record DownloadLatestVersionRequest(string Version, string DownloadUrl) : IRequest<DownloadLatestVersionResponse>;