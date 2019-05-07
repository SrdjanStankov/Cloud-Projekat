using Common;
using System;
using System.ServiceModel;

namespace Container
{
    public class Program
    {
        private static Server server;
        private static string address;

        private static void Main(string[] args) // args[0] == Container-(port)  containerId         // args: containerId, ComputeServerPort
        {
            Console.WriteLine("Container started....");

            ContainerManagement.ContainerId = args[0];
            Console.WriteLine($"Compute server port: {args[1]}");

            using (var factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), $"net.tcp://localhost:{args[1]}"))
            {
                var proxy = factory.CreateChannel();
                address = proxy.AcquireAddress("", ContainerManagement.ContainerId);
                Console.WriteLine($"Container id = {ContainerManagement.ContainerId}");
            }

            Console.WriteLine(address);
            server = new Server(address);

            if (!server.Open())
            {
                Console.WriteLine("Error starting server...");
            }

            Console.ReadKey(true);

            if (!server.Close())
            {
                Console.WriteLine("Error closing server");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
