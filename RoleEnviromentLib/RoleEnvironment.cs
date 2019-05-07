using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoleEnviromentLib
{
    public class RoleEnvironment
    {
        public static string myAddress;

        /// <summary>
        /// Vrednost je vrednost porta na kojoj se WCF server izvrsava
        /// Napomena: zbog jednostavnosti zadatka, moze biti samo jedan WCF server po klijentskom projektu
        /// </summary>
        public static string CurrentRoleInstance(string myAssembly, string containerId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Povratna vrednost je lista portova bratskih instanci.
        /// </summary>
        public static string[] BrotherInstances { get; }
    }
}
