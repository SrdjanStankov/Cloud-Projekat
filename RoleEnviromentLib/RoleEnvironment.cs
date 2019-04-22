using Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoleEnviromentLib
{
    public class RoleEnvironment : IRoleEnvironment
    {
        public static string myAddress;

        /// <summary>
        /// Vrednost je vrednost porta na kojoj se WCF server izvrsava
        /// Napomena: zbog jednostavnosti zadatka, moze biti samo jedan WCF server po klijentskom projektu
        /// </summary>
        public static string CurrentRoleInstance(string myAssembly, string containerId)
        {
            //foreach (var item in ContainerFactory.Instance.Containers)
            //{
            //    if (item.Key == containerId)
            //    {
            //        return item.Value;
            //    }
            //}
            return "";
        }

        public string AcquireAddress(string myAssemblyName, string containerId)
        {
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Proba.txt", myAssemblyName);
            //if (!ContainerFactory.Instance.Containers.ContainsKey(containerId))
            //{
            //    return "";
            //}

            //return ContainerFactory.Instance.Containers[containerId];
            return "";
        }

        string[] IRoleEnvironment.BrotherInstances(string myAssemblyName, string myAddress)
        {
            RoleEnvironment.myAddress = myAddress;
            return BrotherInstances;
        }

        /// <summary>
        /// Povratna vrednost je lista portova bratskih instanci.
        /// </summary>
        public static string[] BrotherInstances
        {
            get
            {
                var retVal = new List<string>();

                //foreach (var item in ContainerFactory.Instance.Containers)
                //{
                //    if (item.Value == myAddress)
                //    {
                //        continue;
                //    }
                //    retVal.Add(item.Value);
                //}

                return retVal.ToArray();
            }
        }
    }
}
