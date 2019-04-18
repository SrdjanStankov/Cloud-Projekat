using System.IO;
using System.Linq;
using static System.Console;
using static System.Environment;

namespace Compute
{
    public class Program
    {
        public static int numberOfContainers = 0;

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
            CopyDllToContainers();
            ContainerFactory.Instance.StartContainers();

            WriteLine("Press key to abort all processes");
            ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();

            WriteLine("Press key to exit");
            ReadKey(true);
        }
        private static void CopyDllToContainers()
        {
            foreach (string item in Directory.GetFiles(ComputeConfigurationContainer.ContainerExePath, "*.dll"))
            {
                if (item.Contains("Common.dll"))
                {
                    continue;
                }
                File.Delete(item);
            }
            string sourceFileName = Directory.GetFiles(ComputeConfigurationContainer.ConfigLocation, "*.dll").FirstOrDefault();
            string[] splited = sourceFileName.Split('\\');
            string fileName = splited[splited.Length - 1];
            string destinationFileName = $@"{ComputeConfigurationContainer.ContainerExePath}\{fileName}";
            File.Copy(sourceFileName, destinationFileName, true);
        }
    }
}
