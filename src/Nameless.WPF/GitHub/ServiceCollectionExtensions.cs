using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nameless.WPF.GitHub;

/// <summary>
///     <see cref="IServiceCollection"/> extension methods.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Register GitHub HTTP client.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="configuration">
    ///     The configuration.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterGitHubHttpClient(this IServiceCollection self, IConfiguration configuration) {
        var section = configuration.GetSection(nameof(GitHubOptions));

        self.Configure<GitHubOptions>(section);

        return self.InnerRegisterGitHubHttpClient();
    }

    /// <summary>
    ///     Register GitHub HTTP client.
    /// </summary>
    /// <param name="self">
    ///     The current <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="configure">
    ///     The configuration action.
    /// </param>
    /// <returns>
    ///     The current <see cref="IServiceCollection"/> so other actions
    ///     can be chained.
    /// </returns>
    public static IServiceCollection RegisterGitHubHttpClient(this IServiceCollection self, Action<GitHubOptions>? configure = null) {
        self.Configure(configure ?? (_ => { }));

        return self.InnerRegisterGitHubHttpClient();
    }

    public static IServiceCollection InnerRegisterGitHubHttpClient(this IServiceCollection self) {
        self.AddHttpClient<IGitHubHttpClient, GitHubHttpClient>((provider, client) => {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", ["2022-11-28"]);

            var url = provider.GetOptions<GitHubOptions>().Value.Api;
            if (string.IsNullOrWhiteSpace(url)) {
                throw new InvalidOperationException("Missing GitHub API URL.");
            }

            client.BaseAddress = new Uri(url);
        });

        return self;
    }
}
