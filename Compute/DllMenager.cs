using Common;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Compute
{
    public static class DllMenager
    {
        public static string CopyDllToContainers()
        {
            foreach (string item in Directory.GetFiles(ComputeConfigurationContainer.ContainerExePath, "*.dll"))
            {
                if (item.Contains("Common.dll"))
                {
                    continue;
                }
                try
                {
                    File.Delete(item);
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("Failed to delete old file...");
                }
            }

            string sourceFileName = Directory.GetFiles(ComputeConfigurationContainer.ConfigLocation, "*.dll").FirstOrDefault();
            string fileName = sourceFileName.Split('\\').Last();
            string destinationFileName = $@"{ComputeConfigurationContainer.ContainerExePath}\{fileName}";
            try
            {
                File.Copy(sourceFileName, destinationFileName, true);
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Failed to copy file...");
            }

            return destinationFileName;
        }

        public static void LoadDllToContainer(string address, string dllPath)
        {
            using (var factory = new ChannelFactory<IContainerManagement>(new NetTcpBinding(), address))
            {
                var proxy = factory.CreateChannel();
                proxy.Load(dllPath);
            }
        }
    }
}
