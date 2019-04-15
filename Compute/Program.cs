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

            ContainerFactory.Instance.StartContainers();

            new FileWatcher().StartWatching();  // periodicno proverava da li se na predefinisanoj lokaciji nalazi novi paket

            // TODO: kopira dll iz paketa na lokaciju container-a


            WriteLine("Press key to abort all processes");
            ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();

            WriteLine("Press key to exit");
            ReadKey(true);
        }
    }
}
