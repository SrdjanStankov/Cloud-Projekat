using Common;
using System.Collections.Generic;

namespace Compute
{
    public class RoleEnviroment : IRoleEnvironment
    {
        public string AcquireAddress(string myAssemblyName, string containerId) => ContainerFactory.Instance.Containers[containerId];

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
