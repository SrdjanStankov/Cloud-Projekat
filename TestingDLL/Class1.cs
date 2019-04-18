using Common;
using System;

namespace TestingDLL
{
    public class Class1 : IWorker
    {
        public void Start(string containerId) => Console.WriteLine("Start pokrenut...");
        public void Stop() => Console.WriteLine("Stop pokrenut...");
    }
}
