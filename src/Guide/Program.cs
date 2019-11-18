using System.Threading.Tasks;
using Ninject;

namespace Guide
{
    class Program
    {
        static async Task Main(string[] args)
            => await new Startup().Kernel.Get<Guide>().Run();
    }
}
