using System;

namespace Compute
{
    public class Program
    {
        public static int numberOfContainers = 0;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            if (!ContainerFactory.Instance.CreateContainers(ComputeConfigurationManipulator.LoadConfiguration() ?? -1))
            {
                Console.WriteLine("Nevalidna konfiguracija");
                Environment.Exit(-100);     // BAD configuration
            }

            ContainerFactory.Instance.StartContainers();

            new FileWatcher().StartWatching();  // periodicno proverava da li se na predefinisanoj lokaciji nalazi novi paket

            Console.WriteLine("Press key to abort all processes");
            Console.ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();

            Console.WriteLine("Press key to exit");
            Console.ReadKey(true);
        }
    }
}
