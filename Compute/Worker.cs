using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compute
{
    public class Worker : IWorker
    {
        public void Start(string containerId) => throw new NotImplementedException();
        public void Stop() => throw new NotImplementedException();
    }
}
