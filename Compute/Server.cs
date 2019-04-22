using Common;
using RoleEnviromentLib;
using System;
using System.ServiceModel;

namespace Compute
{
    public class Server
    {
        private ServiceHost host;

        public Server(int port, Type serverType, Type interfaceType)
        {
            host = new ServiceHost(serverType);
            host.AddServiceEndpoint(interfaceType, new NetTcpBinding(), $"net.tcp://localhost:{port}");
        }

        public bool Open()
        {
            try
            {
                host.Open();
                Console.WriteLine("Server opened...");
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
            try
            {
                host.Close();
                Console.WriteLine("Server closed...");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
