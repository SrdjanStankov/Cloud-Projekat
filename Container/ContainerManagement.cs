using Common;
using System;
using System.Linq;
using System.Reflection;

namespace Container
{
    public class ContainerManagement : IContainerManagement
    {
        public static string ContainerId { get; }

        public string CheckHealth() => "OK";

        public string Load(string assemblyName)
        {
            Assembly dll = null;
            try
            {
                dll = Assembly.LoadFile(assemblyName);
            }
            catch (Exception)
            {
                Console.WriteLine($"Cannot load dll {assemblyName}.");
                return $"Dll {assemblyName} could not be loaded.";
            }

            if (dll is null)
            {
                Console.WriteLine("Error loading dll");
                return "Error loading dll";
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

            return "OK";
        }
    }
}
