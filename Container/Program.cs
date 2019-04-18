using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Container
{
    public class Program
    {
        private static Server server;
        private static int port;
        private static string containerId;

        private static void Main(string[] args)// args[0] == port servera   // args[1] lokacija exe fajla container-a
        {
            Console.WriteLine("Container started....");

            port = GetPort(ref args);
            containerId = $"Container{port}";
            LoadDLL(args);

            server = new Server(port);

            if (!server.Open())
            {
                Console.WriteLine("Error starting server...");
            }

            Console.ReadKey(true);

            if (!server.Close())
            {
                Console.WriteLine("Error closing server");
            }


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        private static void LoadDLL(string[] args)
        {
            string[] files = Directory.GetFiles(args[1]);

            string dllPath = "";

            foreach (string item in files)
            {
                if (item.Contains(".dll") && !item.Contains("Common"))
                {
                    dllPath = item;
                    break;
                }
            }

            Assembly dll = null;
            dll = dllPath == "" ? null : Assembly.LoadFile(dllPath);

            if (dll is null)
            {
                Console.WriteLine("Error loading dll");
                return;
            }

            //var classType = dll.GetExportedTypes().Select(s => s.GetInterfaces().SingleOrDefault()).SingleOrDefault();
            var classType = dll.GetExportedTypes().SingleOrDefault();
            Console.WriteLine(classType);

            if (classType.GetInterfaces().Contains(typeof(IWorker)))
            {
                Console.WriteLine($"DLL -{dll.FullName}- LOADED");
            }
            else
            {
                Console.WriteLine($"DLL does not implements interface {typeof(IWorker)}");
                return;
            }

            dynamic instance = Activator.CreateInstance(classType);
            instance.Start(containerId);
        }

        private static int GetPort(ref string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[5];
                args[0] = "10000";
                args[1] = @"E:\SRKI\FTN\Cloud\Projekat\Container\bin\Debug";
            }


            if (!int.TryParse(args[0], out int port))
            {
                Environment.Exit(-500); // BAD input argument
            }

            Console.WriteLine($"port container-a: {port}");

            return port;
        }
    }
}
