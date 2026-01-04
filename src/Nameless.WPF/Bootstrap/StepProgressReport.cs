namespace Nameless.WPF.Bootstrap;

/// <summary>
///     Represents a step progress report.
/// </summary>
/// <param name="Title">
///     The progress title.
/// </param>
/// <param name="Message">
///     The progress message.
/// </param>
public readonly record struct StepProgressReport(string Title, string Message);