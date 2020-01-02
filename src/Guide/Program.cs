using System.Threading.Tasks;
using Ninject;

namespace Guide
{
    internal sealed class Program
    {
        static async Task Main()
            => await new Startup().Kernel.Get<Guide>().Run();
    }
}
