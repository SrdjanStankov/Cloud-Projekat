using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Compute
{
    internal class Program
    {
        public const int numberOfContainers = 4;
        public static int currentPortNum = 10000;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            var containers = new List<Process>();

            for (int i = 0; i < numberOfContainers; i++)
            {
                containers.Add(new Process());
            }

            foreach (Process containerProcess in containers)
            {
                containerProcess.StartInfo.FileName = "E:\\SRKI\\FTN\\Cloud\\Projekat\\Container\\bin\\Debug\\Container.exe";
                containerProcess.StartInfo.Arguments = currentPortNum++.ToString();
                containerProcess.Start();
            }

            Console.ReadKey(true);

            foreach (Process item in containers)
            {
                if (!item.HasExited)
                {
                    item.Kill();
                }
            }

            Console.ReadKey(true);
        }
    }
}
