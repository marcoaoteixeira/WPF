namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Represents a bootstrapper progress report.
/// </summary>
/// <param name="Step">
///     The current step.
/// </param>
/// <param name="Title">
///     The progress title.
/// </param>
/// <param name="Message">
///     The progress message.
/// </param>
public readonly record struct BootstrapperProgressReport(int Step, string Title, string Message);