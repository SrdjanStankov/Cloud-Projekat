using Common;
using System;
using System.ServiceModel;

namespace Container
{
    public class Server
    {
        private ServiceHost host;

        public Server(string address)
        {
            host = new ServiceHost(typeof(ContainerManagement));
            host.AddServiceEndpoint(typeof(IContainerManagement), new NetTcpBinding(), address);
        }

        public bool Open()
        {
            Console.WriteLine("Opening server...");
            try
            {
                host.Open();
                Console.WriteLine($"Server {host.Description.ServiceType} opened...");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool Close()
        {
            Console.WriteLine("Closing server...");
            try
            {
                host.Close();
                Console.WriteLine($"Server {host.Description.ServiceType} closed...");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
