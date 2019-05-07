using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace Compute
{
    public class ContainerFactory
    {
        public static ContainerFactory Instance { get; } = new ContainerFactory();
        //           ContainerId, Address
        public Dictionary<string, string> Containers { get; } = new Dictionary<string, string>();


        private ushort portNum = 10000;

        //     fullPath with filename
        private List<string> ContainersLocations = new List<string>();

        //               Address, Process
        private Dictionary<string, Process> ContainersProcesses { get; set; } = new Dictionary<string, Process>();

        private ContainerFactory() { }

        private void CreateAndStartContainer(int serverPort, int i)
        {
            while (CheckIfPortInUse(portNum))
            {
                portNum++;
            }

            string address = $"net.tcp://localhost:{portNum}";
            string ContainerId = $"Container-{portNum++.ToString()}";
            string fileName = ContainersLocations[i];
            string arguments = $"{ContainerId} {serverPort}";

            ContainersProcesses.Add(address, new Process() { StartInfo = new ProcessStartInfo(fileName, arguments) });
            Containers.Add(ContainerId, address);
            ContainersProcesses[address].Start();
        }

        public void CopyToNLocations()
        {
            var files = Directory.GetFiles(ComputeConfigurationContainer.ContainerExePath).ToList();

            for (int i = 0; i < 4; i++)
            {
                string path = ComputeConfigurationContainer.LocationForContainers + $@"/Container {i}";
                Directory.CreateDirectory(path);
                files.ForEach(item => {
                    string destFileName = path + $@"/{item.Split('\\').Last()}";
                    File.Copy(item, destFileName, true);
                    if (destFileName.EndsWith(".exe"))
                    {
                        ContainersLocations.Add(destFileName);
                    }
                });
            }
        }

        public void CreateAndStartContainers(int serverPort)
        {
            for (int i = 0; i < 4; i++)
            {
                CreateAndStartContainer(serverPort, i);
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
            }
        }

        public void KillContainer() => KillContainer(Containers.FirstOrDefault().Key);
    }
}
