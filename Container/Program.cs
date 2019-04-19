using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Container
{
    public class Program
    {
        private static Server server;
        private static int port;

        private static void Main(string[] args)// args[0] == port servera   // args[1] lokacija exe fajla container-a
        {
            Console.WriteLine("Container started....");

            port = GetPort(ref args);

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

        private static int GetPort(ref string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[5];
                args[0] = "10000";
            }


            if (!int.TryParse(args[0], out int port))
            {
                Environment.Exit(-500); // BAD input argument
            }

            Console.WriteLine($"Port container-a: {port}");

            return port;
        }
    }
}
