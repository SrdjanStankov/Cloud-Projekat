using Common;
using System;
using System.ServiceModel;

namespace Container
{
    internal class Program
    {
        private static void Main(string[] args)// args[0] == port servera
        {
            Console.WriteLine("Container started....");

            args = new string[2];
            args[0] = 10000.ToString();

            if (!int.TryParse(args[0], out int port))
            {
                Environment.Exit(-500); // BAD input argument
            }

            Console.WriteLine(port);

            try
            {
                var host = new ServiceHost(typeof(Worker));
                host.AddServiceEndpoint(typeof(IWorker), new NetTcpBinding(), $"net.tcp://localhost:{port}");
                host.Open();
                Console.ReadKey(true);
                host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.ReadKey(true);
        }
    }
}
