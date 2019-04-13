using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace Compute
{
    internal class Program
    {
        public static int numberOfContainers = 0;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            numberOfContainers = LoadConfiguration() ?? -1;

            if (!ContainerFactory.Instance.CreateContainers(numberOfContainers))
            {
                Console.WriteLine("Nevalidna konfiguracija");
                Environment.Exit(-100);     // BAD configuration
            }

            ContainerFactory.Instance.StartContainers();

            var t = new Thread(CheckForNewPackage);


            Console.WriteLine("Press key to abort all processes");
            Console.ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();

            Console.WriteLine("Press key to exit");
            Console.ReadKey(true);
        }

        private static void CheckForNewPackage()
        {
            while (true)
            {
                Console.WriteLine("Checking package...");
                // TODO: Proveravanje paketa na predefinisanoj lokaciji

                Thread.Sleep(1000);
            }
        }

        private static int? LoadConfiguration()
        {
            using (var fs = new FileStream("Config.xml", FileMode.OpenOrCreate))
            {
                var serializer = new XmlSerializer(typeof(Config));
                var c = serializer.Deserialize(fs) as Config;

                if (c.InstanceCount > 4 || c.InstanceCount < 1 || c is null)
                {
                    // TODO: Brisanje paketa sa predefinisane lokacije

                    return null;
                }
                else
                {
                    return c.InstanceCount;
                }
            }
        }
    }
}
