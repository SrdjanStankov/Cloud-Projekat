using Common;
using System;
using System.Linq;
using System.Reflection;

namespace Container
{
    public class ContainerManagement : IContainerManagement
    {
        public static string ContainerId { get; set; }

        public static string AssemblyName { get; set; }

        public string CheckHealth() => "OK";

        public string Load(string assemblyName)
        {
            Assembly dll = null;
            try
            {
                Console.WriteLine("Loading dll...");
                dll = Assembly.LoadFile(assemblyName);
            }
            catch (Exception)
            {
                Console.WriteLine($"Cannot load dll {assemblyName}.");
                return $"Dll {assemblyName} could not be loaded.";
            }

            Console.WriteLine("Checking if dll is null..");
            if (dll is null)
            {
                Console.WriteLine("Error loading dll");
                return "Error loading dll";
            }

            Console.WriteLine("Getting types...");
            var classType = dll.GetExportedTypes().SingleOrDefault();
            Console.WriteLine(classType);

            if (classType.GetInterfaces().Contains(typeof(IWorker)))
            {
                Console.WriteLine($"DLL -{dll.FullName}- LOADED");
            }
            else
            {
                Console.WriteLine($"DLL does not implements interface {typeof(IWorker)}");
                return $"DLL does not implements interface {typeof(IWorker)}";
            }

            try
            {
                dynamic instance = Activator.CreateInstance(classType);
                instance.Start(ContainerId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            AssemblyName = assemblyName;
            return "OK";
        }
    }
}
