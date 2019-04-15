using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;

namespace Compute
{
    internal class Program
    {
        public static int numberOfContainers = 0;

        private static string configLocation = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["PredefinedLocation"].Value;
        private static bool canFireChangeEvent = true;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press to start process");
            Console.ReadKey(true);

            if (!ContainerFactory.Instance.CreateContainers(LoadConfiguration() ?? -1))
            {
                Console.WriteLine("Nevalidna konfiguracija");
                Environment.Exit(-100);     // BAD configuration
            }

            ContainerFactory.Instance.StartContainers();

            new Thread(CheckForNewPackage) { IsBackground = true }.Start();

            var watcher = new FileSystemWatcher
            {
                Filter = "*.dll",
                Path = configLocation
            };
            watcher.Changed += DllChanged;
            watcher.Created += DllCreated;
            watcher.Deleted += DllDeleted;
            watcher.EnableRaisingEvents = true;



            Console.WriteLine("Press key to abort all processes");
            Console.ReadKey(true);

            ContainerFactory.Instance.KillLiveContainers();

            Console.WriteLine("Press key to exit");
            Console.ReadKey(true);
        }

        // TODO: reaguj na promenu dll fajla
        private static void DllDeleted(object sender, FileSystemEventArgs e) => Console.WriteLine("DLL has been removed");
        private static void DllCreated(object sender, FileSystemEventArgs e) => Console.WriteLine("DLL has been created");

        private static void DllChanged(object sender, FileSystemEventArgs e)
        {
            canFireChangeEvent = !canFireChangeEvent;
            if (!canFireChangeEvent)
            {
                return;
            }
            Console.WriteLine("DLL Has changed");
        }

        private static void CheckForNewPackage()
        {
            while (true)
            {
                Console.WriteLine("Checking package...");
                // TODO: Proveravanje paketa na predefinisanoj lokaciji

                /*
                 * Nesto ovako npr.
                 * AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => typeof(IDomainEntity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(x => x.Name).ToList();
                 */

                Thread.Sleep(5000);
            }
        }

        private static int? LoadConfiguration()
        {
            using (var fs = new FileStream(configLocation + @"\Config.xml", FileMode.Open))
            {
                var xmlReader = XmlReader.Create(fs);

                xmlReader.ReadStartElement("Config");
                xmlReader.Read();
                int instanceCount = int.Parse(xmlReader.ReadInnerXml());

                if (instanceCount > 4 || instanceCount < 1)
                {
                    // TODO: Brisanje paketa sa predefinisane lokacije
                    DeletePackage();
                    return null;
                }
                else
                {
                    return instanceCount;
                }
            }
        }

        private static void DeletePackage() => throw new NotImplementedException();
    }
}
