using System.Collections.Generic;
using System.Diagnostics;

namespace Compute
{
    public class ContainerFactory
    {
        public static ContainerFactory Instance { get; private set; }

        private const string containerExePath = "E:\\SRKI\\FTN\\Cloud\\Projekat\\Container\\bin\\Debug\\Container.exe";

        private List<Process> containers = new List<Process>();
        private int portNum = 10000;

        private ContainerFactory() { }

        static ContainerFactory() => Instance = new ContainerFactory();

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
                containerProcess.StartInfo.FileName = containerExePath;
                containerProcess.StartInfo.Arguments = portNum++.ToString();
                containerProcess.Start();
            }
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
