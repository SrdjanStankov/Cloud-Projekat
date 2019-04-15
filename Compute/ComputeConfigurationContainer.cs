using System.Configuration;

namespace Compute
{
    public static class ComputeConfigurationContainer
    {
        public static string ContainerExePath { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["ContainerExePath"].Value;
        public static string ConfigLocation { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["PredefinedLocation"].Value;
    }
}
