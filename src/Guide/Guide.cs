using System.Threading.Tasks;
using Guide.Connection;
using Guide.Handlers;
using Guide.Services;

namespace Guide
{
    public class Guide
    {
        private IConnection connection;
        private readonly ICommandHandler commandHandler;

        public Guide(IConnection connection, ICommandHandler commandHandler)
        {
            this.connection = connection;
            this.commandHandler = commandHandler;
        }

        public async Task Run()
        {
            await connection.Connect();

            // No need to keep the connection
            // in memory after connecting.
            connection = null;

            await commandHandler.InitializeAsync();
            await Task.Delay(-1);
        }
    }
}
