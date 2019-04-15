using Common;
using System;

namespace Container
{
    public class Worker : IWorker
    {
        public void Start(string containerId) => Console.WriteLine("Pozvana je start metoda");
        public void Stop() => throw new NotImplementedException();
    }
}
