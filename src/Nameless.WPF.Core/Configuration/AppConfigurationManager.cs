using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nameless.IO.FileSystem;
using Nameless.WPF.Internals;

namespace Nameless.WPF.Configuration;

/// <summary>
///     Default implementation of <see cref="IAppConfigurationManager"/>.
/// </summary>
public class AppConfigurationManager : IAppConfigurationManager {
    private const string APP_CONFIGURATION_FILE = "app.config";

    private readonly Lazy<Dictionary<string, JsonElement>> _appConfiguration;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<AppConfigurationManager> _logger;

    private Dictionary<string, JsonElement> AppConfiguration => _appConfiguration.Value;

    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="AppConfigurationManager"/> class.
    /// </summary>
    /// <param name="fileSystem">
    ///     The file system.
    /// </param>
    /// <param name="logger">
    ///     The logger.
    /// </param>
    public AppConfigurationManager(IFileSystem fileSystem, ILogger<AppConfigurationManager> logger) {
        _fileSystem = Guard.Against.Null(fileSystem);
        _logger = Guard.Against.Null(logger);
        _appConfiguration = new Lazy<Dictionary<string, JsonElement>>(GetAppConfiguration);
    }

    /// <inheritdoc />
    public bool TryGet<TValue>(string name, [NotNullWhen(returnValue: true)] out TValue? output) {
        Guard.Against.Null(name);

        output = default;

        var element = AppConfiguration.GetValueOrDefault(name);
        if (element.ValueKind == JsonValueKind.Undefined) {
            return false;
        }

        try {
            var value = element.Deserialize<TValue>();
            if (value is not null) {
                output = value;

                return true;
            }
        }
        catch (Exception ex) { _logger.TryGetValueFailure(name, typeof(TValue), ex); }

        return false;
    }

    /// <inheritdoc />
    public void Set<TValue>(string name, TValue value) {
        Guard.Against.NullOrWhiteSpace(name);

        AppConfiguration[name] = JsonSerializer.SerializeToElement(value);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken) {
        try {
            var file = _fileSystem.GetFile(APP_CONFIGURATION_FILE);
            var json = JsonSerializer.SerializeToUtf8Bytes(AppConfiguration);

            await using var stream = file.Open(FileMode.Truncate);
            await stream.WriteAsync(json, cancellationToken)
                        .SuppressContext();
        }
        catch (Exception ex) { _logger.SaveAppConfigurationFailure(ex); }
    }

    private Dictionary<string, JsonElement> GetAppConfiguration() {
        var file = _fileSystem.GetFile(APP_CONFIGURATION_FILE);

        if (!file.Exists) { return []; }

        using var stream = file.Open();

        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream) ?? [];
    }
}