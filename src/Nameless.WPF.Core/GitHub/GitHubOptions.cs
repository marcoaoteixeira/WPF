namespace Nameless.WPF.GitHub;

/// <summary>
///     Defines the GitHub options for the application.
/// </summary>
public class GitHubOptions {
    /// <summary>
    ///     The application GitHub owner.
    /// </summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    ///     The application GitHub repository name.
    /// </summary>
    public string Repository { get; set; } = string.Empty;
}
