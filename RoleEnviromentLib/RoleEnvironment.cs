using Common;
using System.ServiceModel;

namespace RoleEnviromentLib
{
    public class RoleEnvironment
    {
        /// <summary>
        /// Vrednost je vrednost porta na kojoj se WCF server izvrsava
        /// Napomena: zbog jednostavnosti zadatka, moze biti samo jedan WCF server po klijentskom projektu
        /// </summary>
        public static string CurrentRoleInstance(string myAssembly, string containerId)
        {
            using (var factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), "net.tcp://localhost:15000"))
            {
                var proxy = factory.CreateChannel();
                return proxy.AcquireAddress("RoleEnviroment", containerId);
            }
        }

        /// <summary>
        /// Povratna vrednost je lista portova bratskih instanci.
        /// </summary>
        public static string[] BrotherInstances
        {
            get
            {
                using (var factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), "net.tcp://localhost:15000"))
                {
                    var proxy = factory.CreateChannel();
                    return proxy.BrotherInstances("RoleEnviroment", "");
                }
            }
        }

    }
}
