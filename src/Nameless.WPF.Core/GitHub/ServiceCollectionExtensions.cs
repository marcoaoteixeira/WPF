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
    public static IServiceCollection RegisterGitHubHttpClient(this IServiceCollection self, IConfiguration? configuration = null) {
        if (configuration is null) {
            return self.InnerRegisterGitHubHttpClient();
        }

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
        self.AddHttpClient<IGitHubHttpClient, GitHubHttpClient>((provider, httpClient) => {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", ["2022-11-28"]);

            //var applicationContext = provider.GetRequiredService<IApplicationContext>();
            //var productHeaderValue = new ProductHeaderValue(applicationContext.ApplicationName, applicationContext.Version);
            //var userAgent = new ProductInfoHeaderValue(productHeaderValue);
            //httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

            httpClient.BaseAddress = new Uri("https://api.github.com");
        });

        return self;
    }
}
