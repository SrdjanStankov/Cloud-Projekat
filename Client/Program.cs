using Common;
using System;
using System.ServiceModel;

namespace Client
{
    public class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("Unesite broj instanci");
                if (!int.TryParse(Console.ReadLine(), out int instance))
                {
                    Console.WriteLine("Niste uneli broj!");
                    continue;
                }

                var proxy = new ChannelFactory<IComputeManagement>(new NetTcpBinding(), $"net.tcp://localhost:{15000 + 10}").CreateChannel();
                proxy.Scale("", instance);
            }
        }
    }
}
