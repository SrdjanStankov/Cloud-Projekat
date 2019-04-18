using Common;
using System;
using System.ServiceModel;

namespace Container
{
    public class Server
    {
        private ServiceHost host;

        public Server(int port)
        {
            host = new ServiceHost(typeof(ContainerManagement));
            host.AddServiceEndpoint(typeof(IContainerManagement), new NetTcpBinding(), $"net.tcp://localhost/{port}");
        }

        public bool Open()
        {

            try
            {
                host.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Close()
        {

            try
            {
                host.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
