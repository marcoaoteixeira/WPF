namespace Nameless.WPF.GitHub;

/// <summary>
///     Defines the GitHub options for the application.
/// </summary>
public class GitHubOptions {
    /// <summary>
    ///     Gets or sets the API URL.
    /// </summary>
    /// <remarks>
    ///     Default is <a href="https://api.github.com">GitHub API</a>.
    /// </remarks>
    public string Api { get; set; } = "https://api.github.com";

    /// <summary>
    ///     The application GitHub owner.
    /// </summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    ///     The application GitHub repository name.
    /// </summary>
    public string Repository { get; set; } = string.Empty;
}
