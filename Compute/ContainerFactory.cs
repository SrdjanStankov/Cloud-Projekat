using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Compute
{
    public class ContainerFactory
    {
        private ContainerFactory() { }
        public static ContainerFactory Instance { get; } = new ContainerFactory();

        private ushort portNum = 10000;

        //           ContainerId, Address
        public Dictionary<string, string> Containers { get; } = new Dictionary<string, string>();

        //               Address, Process
        private Dictionary<string, Process> ContainersProcesses { get; set; } = new Dictionary<string, Process>();

        public void CreateAndStartContainer(int serverPort)
        {
            while (CheckIfPortInUse(portNum))
            {
                portNum++;
            }

            string address = $"net.tcp://localhost:{portNum}";
            string fileName = $@"{ComputeConfigurationContainer.ContainerExePath}\Container.exe";
            string ContainerId = $"Container-{portNum++.ToString()}";
            string arguments = $"{ContainerId} {serverPort}";

            ContainersProcesses.Add(address, new Process() { StartInfo = new ProcessStartInfo(fileName, arguments) });
            Containers.Add(ContainerId, address);
            ContainersProcesses[address].Start();
        }

        public void CreateAndStartContainer() => CreateAndStartContainer(portNum);

        public void CreateAndStartContainers(int numberOfContainers, int startingServerPort)
        {
            for (int i = 0; i < numberOfContainers; i++)
            {
                CreateAndStartContainer(startingServerPort++);
            }
        }

        public void RestartContainer(string containerId)
        {
            if (ContainersProcesses[Containers[containerId]].HasExited)
            {
                ContainersProcesses[Containers[containerId]].Start();
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

        public void KillAllLiveContainers()
        {
            foreach (var item in ContainersProcesses)
            {
                if (!item.Value.HasExited)
                {
                    item.Value.Kill();
                }
            }
            Containers.Clear();
            ContainersProcesses.Clear();
        }

        public void KillContainer(string containerId)
        {
            if (!ContainersProcesses[Containers[containerId]].HasExited)
            {
                ContainersProcesses[Containers[containerId]].Kill();
                ContainersProcesses.Remove(Containers[containerId]);
                Containers.Remove(containerId);
            }
        }

        public void KillContainer() => KillContainer(Containers.FirstOrDefault().Key);
    }
}
