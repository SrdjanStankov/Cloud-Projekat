﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Compute
{
    public class ContainerFactory
    {
        public static ContainerFactory Instance { get; } = new ContainerFactory();

        private List<Process> containers = new List<Process>();
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
                containerProcess.StartInfo.FileName = ComputeConfigurationContainer.ContainerExePath;
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