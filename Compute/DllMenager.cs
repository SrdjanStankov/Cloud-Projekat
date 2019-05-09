using Common;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Compute
{
    public static class DllMenager
    {
        public static string CopyDllToMainContainerLocation()
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
                catch (Exception)
                {
                    Console.WriteLine("Failed to delete old file...");
                }
            }

            var sourceFileName = Directory.GetFiles(ComputeConfigurationContainer.ConfigLocation, "*.dll").ToList();
            string dest = "";
            sourceFileName.ForEach(item =>
            {
                string filename = item.Split('\\').Last();
                string destinationFileName = $@"{ComputeConfigurationContainer.ContainerExePath}\{filename}";
                try
                {
                    File.Copy(item, destinationFileName, true);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to copy file...");
                }

                dest = destinationFileName;
            });
            return dest;
        }

        public static void LoadDllToContainer(string address, string dllPath)
        {
            using (var factory = new ChannelFactory<IContainerManagement>(new NetTcpBinding(), address))
            {
                try
                {
                    var proxy = factory.CreateChannel();
                    Console.WriteLine(proxy.Load(dllPath));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
