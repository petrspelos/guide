using System.Threading.Tasks;
using Octokit;

namespace Guide
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var github = new GitHubClient(new ProductHeaderValue("Guide"));
            //var user = await github.User.Get("petrspelos");
            //var a = await github.Activity.Events.GetAllUserPerformedPublic("petrspelos");
            await InversionOfControl.Container.GetInstance<Guide>().Run();
        }
    }
}
