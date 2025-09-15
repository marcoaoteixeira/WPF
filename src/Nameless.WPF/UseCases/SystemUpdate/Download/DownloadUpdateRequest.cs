﻿using Nameless.Mediator.Requests;

namespace Nameless.WPF.UseCases.SystemUpdate.Download;

public record DownloadUpdateRequest(string Version, string DownloadUrl) : IRequest<DownloadUpdateResponse>;