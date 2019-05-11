using Common;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Compute
{
    public static class DllMenager
    {
        public static void ClearContainersOfOldDlls()
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    foreach (string item in Directory.GetFiles($@"{GetLoacationForContainer(i)}", "*.dll"))
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
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Directori not found.");
                    Console.WriteLine("Cannot delete old files.");
                    Console.WriteLine("Skipping delete operation...");
                    return;
                }
            }
        }

        public static void CopyDllsToNContainers(int numberOfContainers)
        {
            var sourceFileName = Directory.GetFiles(ComputeConfigurationContainer.ConfigLocation, "*.dll").ToList();

            for (int i = 0; i < numberOfContainers; i++)
            {
                sourceFileName.ForEach(item =>
                {
                    string filename = item.Split('\\').Last();
                    string destinationFileName = $@"{GetLoacationForContainer(i)}/{filename}";
                    try
                    {
                        File.Copy(item, destinationFileName, true);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to copy file...");
                    }
                });
            }
        }

        public static void LoadDllToContainers()
        {
            for (int i = 0; i < 4; i++)
            {
                string address = ContainerFactory.Instance.Containers.ElementAt(i).Value;

                using (var factory = new ChannelFactory<IContainerManagement>(new NetTcpBinding(), address))
                {
                    try
                    {
                        var proxy = factory.CreateChannel();
                        string filename = "";
                        Directory.GetFiles(GetLoacationForContainer(i), "*.dll").ToList().ForEach(item =>
                        {
                            if (!item.Contains("Common.dll") && !item.Contains("RoleEnviromentLib.dll"))
                            {
                                filename = item.Split('\\').Last();
                                return;
                            }
                        });
                        if (!string.IsNullOrEmpty(filename) && !string.IsNullOrWhiteSpace(filename))
                        {
                            string dllPath = $@"{GetLoacationForContainer(i)}/{filename}"; 
                            Console.WriteLine(proxy.Load(dllPath));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                } 
            }
        }

        private static string GetLoacationForContainer(int i) => $"{ComputeConfigurationContainer.LocationForContainers}\\Container {i}";
    }
}
