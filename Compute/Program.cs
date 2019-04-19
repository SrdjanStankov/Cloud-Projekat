using Common;
using System;
using System.ServiceModel;
using System.Threading;

namespace Compute
{
    public class Program
    {
        public static int numberOfContainers = 0;
        private static string copiedDllPath;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            if (!ContainerFactory.Instance.CreateAndStartContainers(ComputeConfigurationManipulator.LoadConfiguration() ?? -1))
            {
                Console.WriteLine("Nevalidna konfiguracija");
                Console.ReadKey();
                return;
            }

            new FileWatcher().StartWatching();  // periodicno proverava da li se na predefinisanoj lokaciji nalazi novi paket

            copiedDllPath = DllMenager.CopyDllToContainers();

            foreach (var item in ContainerFactory.Instance.Containers)
            {
                DllMenager.LoadDllToContainer(item.Key, copiedDllPath);
            }

            var t = new Thread(CheckHealth) { IsBackground = true };
            t.Start();

            Console.WriteLine("Press key to abort all processes");
            Console.ReadKey(true);

            t.Abort();
            ContainerFactory.Instance.KillLiveContainers();

            Console.WriteLine("Press key to exit");
            Console.ReadKey(true);
        }

        private static void CheckHealth()
        {
            while (true)
            {
                Console.WriteLine("Checking health of containers...");
                foreach (var item in ContainerFactory.Instance.Containers)
                {
                    try
                    {
                        using (var factory = new ChannelFactory<IContainerManagement>(new NetTcpBinding(), item.Key))
                        {
                            var proxy = factory.CreateChannel();
                            proxy.CheckHealth();
                        }
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        Console.WriteLine($"Coontainer on address {item.Key} not responding...");
                        Console.WriteLine("Killing container...");
                        ContainerFactory.Instance.KillContainer(item.Key);
                        Console.WriteLine("Restarting container");
                        ContainerFactory.Instance.RestartContainer(item.Key);
                        DllMenager.LoadDllToContainer(item.Key, copiedDllPath);
                    }
                    catch (EndpointNotFoundException)
                    {
                        Console.WriteLine($"------------------------- Endpoint not found --------------{item.Key}");
                        Console.WriteLine($"Trying to reboot container on address {item.Key} ...");
                        ContainerFactory.Instance.RestartContainer(item.Key);
                        DllMenager.LoadDllToContainer(item.Key, copiedDllPath);
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
