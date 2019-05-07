using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compute
{
    public class RoleEnviroment : IRoleEnvironment
    {
        public string AcquireAddress(string myAssemblyName, string containerId) => $"net.tcp://localhost:{containerId.Split('-').Last()}";

        public string[] BrotherInstances(string myAssemblyName, string myAddress)
        {
            var retVal = new List<string>();

            foreach (var item in ContainerFactory.Instance.Containers)
            {
                if (item.Value == myAddress)
                {
                    continue;
                }
                retVal.Add(item.Value);
            }

            return retVal.ToArray();
        }
    }
}
