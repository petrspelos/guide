using Octokit;

namespace Guide.GitWeek
{
    public class GithubClientFactory
    {
        public static GitHubClient GetDefault()
        {
            return new GitHubClient(new ProductHeaderValue("Guide"));
        }
    }
}