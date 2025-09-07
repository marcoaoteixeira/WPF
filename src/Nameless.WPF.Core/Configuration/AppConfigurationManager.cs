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
public class AppConfigurationManager : IAppConfigurationManager, IDisposable, IAsyncDisposable {
    private const string APP_CONFIGURATION_FILE = "app.config";

    private readonly Lazy<Dictionary<string, JsonElement>> _appConfiguration;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<AppConfigurationManager> _logger;

    private bool _disposed;

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
            var json = JsonSerializer.Serialize(AppConfiguration);

            await using var stream = file.Open();
            await using var writer = new StreamWriter(stream);

            await writer.WriteLineAsync(json);
            await writer.FlushAsync(cancellationToken);

            writer.Close();
            stream.Close();
        }
        catch (Exception ex) { _logger.SaveAppConfigurationFailure(ex); }
    }

    /// <inheritdoc />
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().SuppressContext();

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Dispose the current instance.
    /// </summary>
    /// <param name="disposing">
    ///     Whether it should dispose managed resources.
    /// </param>
    protected virtual void Dispose(bool disposing) {
        if (_disposed) { return; }

        if (disposing) {
            /* dispose managed resource */
        }

        /* dispose unmanaged resource */

        _disposed = true;
    }

    protected async ValueTask DisposeAsyncCore() {
        await SaveChangesAsync(CancellationToken.None);
    }

    private Dictionary<string, JsonElement> GetAppConfiguration() {
        var file = _fileSystem.GetFile(APP_CONFIGURATION_FILE);

        if (!file.Exists) { return []; }

        using var stream = file.Open();

        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream) ?? [];
    }
}