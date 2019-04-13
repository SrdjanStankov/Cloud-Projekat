using System;

namespace Common
{
    public class RoleEnvironment
    {
        /// <summary>
        /// Vrednost je vrednost porta na kojoj se WCF server izvrsava
        /// Napomena: zbog jednostavnosti zadatka, moze biti samo jedan WCF server po klijentskom projektu
        /// </summary>
        public static string CurrentRoleInstance(string myAssembly, string containerId) => throw new NotImplementedException();

        /// <summary>
        /// Povratna vrednost je lista portova bratskih instanci.
        /// </summary>
        public static string[] BrotherInstances { get; }
    }
}
