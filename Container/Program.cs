using System;

namespace Container
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Container started....");

            if (!int.TryParse(args[0], out int res))
            {
                Environment.Exit(-500); // BAD input argument
            }

            Console.WriteLine(res);

            Console.ReadKey(true);
        }
    }
}
