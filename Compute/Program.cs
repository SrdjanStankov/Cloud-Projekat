using System.Collections.Generic;
using System.IO;
using static System.Console;
using static System.Environment;

namespace Compute
{
    public class Program
    {
        static List<Server> servers = new List<Server>();

        private static void Main(string[] args)
        {
            WriteLine("Press to start process");
            ReadKey(true);

            if (!ContainerFactory.Instance.CreateContainers(ComputeConfigurationManipulator.LoadConfiguration() ?? -1))
            {
                WriteLine("Nevalidna konfiguracija");
                Exit(-100);     // BAD configuration
            }

            new FileWatcher().StartWatching();  // periodicno proverava da li se na predefinisanoj lokaciji nalazi novi paket

            CreateServers();

            OpenServers();

            CopyDllToContainers();

            ContainerFactory.Instance.StartContainers();

            WriteLine("Press key to abort all processes");
            ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();
            CloseServers();

            WriteLine("Press key to exit");
            ReadKey(true);
        }

        private static void CloseServers()
        {
            foreach (var item in servers)
            {
                item.Close();
            }
        }

        private static void OpenServers()
        {
            foreach (var item in servers)
            {
                item.Open();
            }
        }

        private static void CreateServers()
        {
            foreach (var item in ContainerFactory.Instance.Addresses)
            {
                servers.Add(new Server(item));
            }
        }

        private static void CopyDllToContainers()
        {
            foreach (var item in Directory.GetFiles(ComputeConfigurationContainer.ContainerExePath.Replace("Container.exe", ""), "*.dll"))
            {
                File.Delete(item);
            }
            string sourceFileName = Directory.GetFiles(ComputeConfigurationContainer.ConfigLocation, "*.dll")[0];
            string[] splited = sourceFileName.Split('\\');
            string fileName = splited[splited.Length - 1];
            string destinationFileName = ComputeConfigurationContainer.ContainerExePath.Replace("Container.exe", fileName);
            File.Copy(sourceFileName, destinationFileName, true);
        }
    }
}
