using System.Diagnostics.CodeAnalysis;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public readonly record struct DownloadedUpdateMetadata(string? FilePath) {
    [MemberNotNullWhen(returnValue: true, nameof(FilePath))]
    public bool IsUpdateAvailable => !string.IsNullOrWhiteSpace(FilePath);
}