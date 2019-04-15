namespace Common
{
    public interface IContainerManagement
    {
        string Load(string assemblyName);
        string CheckHealth();
    }
}
