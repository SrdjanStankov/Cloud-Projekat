using Common;
using System.Net.NetworkInformation;
using System.ServiceModel;

namespace Compute
{
    public class Server
    {
        private ServiceHost host = new ServiceHost(typeof(Worker));

        public Server(string address)
        {
            host.AddServiceEndpoint(typeof(IWorker), new NetTcpBinding(), address);
        }

        public static bool CheckIfPortInUse(int port)
        {
            foreach (var item in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                if (item.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        public void Open() => host.Open();

        public void Close() => host.Close();
    }
}
