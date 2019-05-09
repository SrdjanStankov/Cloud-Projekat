using Common;
using RoleEnviromentLib;
using System;
using System.Linq;
using System.Threading;

namespace TestingDll
{
    public class Class1 : IWorker
    {
        public void Start(string containerId)
        {
            Console.WriteLine("Pokrecem Start metodu");
            Console.WriteLine($"ContainerId = {containerId}");

            while (true)
            {
                Console.WriteLine($"Current Role Instance: {RoleEnvironment.CurrentRoleInstance("Class1", containerId)}");
                RoleEnvironment.BrotherInstances.ToList().ForEach(i => Console.WriteLine(i));
                Thread.Sleep(5000);
            }
        }

        public void Stop() => throw new NotImplementedException();
    }
}
