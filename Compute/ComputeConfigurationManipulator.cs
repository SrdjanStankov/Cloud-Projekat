using System;
using System.IO;
using System.Xml;

namespace Compute
{
    


    public static class ComputeConfigurationManipulator
    {
        public static int? LoadConfiguration()
        {
            using (var fs = new FileStream(ComputeConfigurationContainer.ConfigLocation + @"\Config.xml", FileMode.Open))
            {
                var xmlReader = XmlReader.Create(fs);

                xmlReader.ReadStartElement("Config");
                xmlReader.Read();
                int instanceCount = int.Parse(xmlReader.ReadInnerXml());

                if (instanceCount > 4 || instanceCount < 1)
                {
                    // TODO: Brisanje paketa sa predefinisane lokacije
                    DeletePackage();
                    return null;
                }
                else
                {
                    return instanceCount;
                }
            }
        }

        private static void DeletePackage() => throw new NotImplementedException();
    }
}
