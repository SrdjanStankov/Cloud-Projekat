using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Compute
{
    public class ContainerFactory
    {
        public static ContainerFactory Instance { get; } = new ContainerFactory();

        private List<Process> containers = new List<Process>();
        public List<string> Addresses { get; } = new List<string>();
        private int portNum = 10000;

        private ContainerFactory() { }

        public bool CreateContainers(int numberOfContainers)
        {
            bool sucess = false;
            for (int i = 0; i < numberOfContainers; i++)
            {
                containers.Add(new Process());
                sucess = true;
            }
            return sucess;
        }

        public void StartContainers()
        {
            foreach (Process containerProcess in containers)
            {
                while (CheckIfPortInUse(portNum))
                {
                    portNum++;
                }

                Addresses.Add($"net.tcp://localhost:{portNum}");
                containerProcess.StartInfo.FileName = ComputeConfigurationContainer.ContainerExePath;
                containerProcess.StartInfo.Arguments = portNum++.ToString();
                containerProcess.Start();
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
            foreach (Process item in containers)
            {
                if (!item.HasExited)
                {
                    item.Kill();
                }
            }
        }
    }
}
