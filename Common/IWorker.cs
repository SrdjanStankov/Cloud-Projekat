using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IWorker
    {
        [OperationContract]
        void Start(string containerId);
        [OperationContract]
        void Stop();
    }
}
