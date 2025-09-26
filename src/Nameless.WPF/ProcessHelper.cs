using System.Diagnostics;

namespace Nameless.WPF;

/// <summary>
///     Helper that can start process.
/// </summary>
public static class ProcessHelper {
    private const string EXPLORER_APP = "explorer.exe";
    private const string NOTEPAD_APP = "notepad.exe";

    /// <summary>
    ///     Opens the specified folder in Windows Explorer.
    /// </summary>
    /// <param name="directoryPath">
    ///     The directory path.
    /// </param>
    public static void OpenDirectory(string directoryPath) {
        Guard.Against.NullOrWhiteSpace(directoryPath);

        using var process = Process.Start(fileName: EXPLORER_APP,
                                          arguments: directoryPath);
    }

    /// <summary>
    ///     Opens the text file using the Notepad.
    /// </summary>
    /// <param name="filePath">
    ///     The file path.
    /// </param>
    public static void OpenTextFile(string filePath) {
        Guard.Against.NullOrWhiteSpace(filePath);

        using var process = Process.Start(fileName: NOTEPAD_APP,
                                          arguments: filePath);
    }

    /// <summary>
    ///     Opens the specified file using the default application.
    /// </summary>
    /// <param name="filePath">
    ///     The file path.
    /// </param>
    public static void OpenFile(string filePath) {
        Guard.Against.NullOrWhiteSpace(filePath);

        using var process = Process.Start(fileName: EXPLORER_APP,
                                          arguments: filePath);
    }
}
