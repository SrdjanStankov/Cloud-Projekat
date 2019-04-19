using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Compute
{
    public class ContainerFactory
    {
        public static ContainerFactory Instance { get; } = new ContainerFactory();

        //private List<Process> containers = new List<Process>();
        //public List<string> Addresses { get; } = new List<string>();

        public Dictionary<string, Process> Containers { get; } = new Dictionary<string, Process>();
        private ushort portNum = 10000;

        private ContainerFactory() { }

        public bool CreateAndStartContainers(int numberOfContainers)
        {
            bool sucess = false;
            for (int i = 0; i < numberOfContainers; i++)
            {
                while (CheckIfPortInUse(portNum))
                {
                    portNum++;
                }

                string address = $"net.tcp://localhost:{portNum}";
                string fileName = $@"{ComputeConfigurationContainer.ContainerExePath}\Container.exe";
                string arguments = $"{portNum++.ToString()}";

                Containers.Add(address, new Process() { StartInfo = new ProcessStartInfo(fileName, arguments) });
                Containers[address].Start();

                System.Console.WriteLine($"Starting container on address {Containers.Last().Key}");
                sucess = true;
            }
            return sucess;
        }

        public void RestartContainer(string address)
        {
            if (Containers[address].HasExited)
            {
                Containers[address].Start();
            }
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

        public void KillLiveContainers()
        {
            foreach (var item in Containers)
            {
                if (!item.Value.HasExited)
                {
                    item.Value.Kill();
                }
            }

        }

        internal void KillContainer(string key)
        {
            if (!Containers[key].HasExited)
            {
                Containers[key].Kill();
            }
        }
    }
}
