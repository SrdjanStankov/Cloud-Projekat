using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IContainerManagement
    {
        [OperationContract]
        string Load(string assemblyName);
        [OperationContract]
        string CheckHealth();
    }
}
