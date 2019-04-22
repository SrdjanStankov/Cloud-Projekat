using Common;
using RoleEnviromentLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;

namespace Compute
{
    public class Program
    {
        public static int numberOfContainers = 0;
        private static string copiedDllPath;
        private static int roleServerStartingPort = 15000;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            if (ComputeConfigurationManipulator.LoadConfiguration() is null)
            {
                Console.WriteLine("Nevalidna konfiguracija");
                Console.ReadKey();
                return;
            }

            var roleServer = new Server(port: roleServerStartingPort, serverType: typeof(RoleEnvironment), interfaceType: typeof(IRoleEnvironment));
            var roleServer2 = new Server(port: roleServerStartingPort + 1, serverType: typeof(RoleEnvironment), interfaceType: typeof(IRoleEnvironment));

            var scaleServer = new Server(port: roleServerStartingPort + 10, serverType: typeof(ComputeMenagment), interfaceType: typeof(IComputeManagement));
            roleServer.Open();
            roleServer2.Open();

            scaleServer.Open();

            ContainerFactory.Instance.CreateAndStartContainers(ComputeConfigurationManipulator.LoadConfiguration().Value, roleServerStartingPort);

            new FileWatcher().StartWatching();  // periodicno proverava da li se na predefinisanoj lokaciji nalazi novi paket

            copiedDllPath = DllMenager.CopyDllToContainers();

            foreach (var item in ContainerFactory.Instance.Containers)
            {
                DllMenager.LoadDllToContainer(item.Value, copiedDllPath);
            }

            var t = new Thread(CheckHealth) { IsBackground = true };
            t.Start();

            Console.WriteLine("Press key to abort all processes");
            Console.ReadKey(true);

            t.Abort();
            ContainerFactory.Instance.KillAllLiveContainers();

            roleServer.Close();
            roleServer2.Close();

            scaleServer.Close();

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
                        using (var factory = new ChannelFactory<IContainerManagement>(new NetTcpBinding(), item.Value))
                        {
                            var proxy = factory.CreateChannel();
                            proxy.CheckHealth();
                        }
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        Console.WriteLine($"Coontainer on address {item.Value} not responding.");
                        Console.WriteLine("Killing container...");
                        ContainerFactory.Instance.KillContainer(item.Key);
                        Console.WriteLine("Restarting container...");
                        ContainerFactory.Instance.RestartContainer(item.Key);
                        DllMenager.LoadDllToContainer(item.Value, copiedDllPath);
                    }
                    catch (EndpointNotFoundException)
                    {
                        Console.WriteLine($"------------------------- Endpoint not found --------------{item.Value}");
                        Console.WriteLine($"Trying to reboot container on address {item.Value} ...");
                        ContainerFactory.Instance.RestartContainer(item.Key);
                        DllMenager.LoadDllToContainer(item.Value, copiedDllPath);
                    }
                }
                Console.WriteLine("All OK.");
                Thread.Sleep(5000);
            }
        }
    }
}
