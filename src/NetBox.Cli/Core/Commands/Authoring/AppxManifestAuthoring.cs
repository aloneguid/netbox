using System.IO;
using System.Xml;
using System;

namespace NetBox.Cli.Core.Commands.Authoring
{
   class AppxManifestAuthoring : IFileAuthoring
   {
      public string TypeName => "appx manifest";

      public void Update(string path, ISettings settings)
      {
         Console.WriteLine("loading {path}", path);
         var xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(File.ReadAllText(path));

         Console.WriteLine("searching for Package/Identity...");

         //ignore namespaces in xml
         XmlNode identityNode = xmlDoc.SelectSingleNode("*[local-name()='Package']/*[local-name()='Identity']");
         if(identityNode == null)
         {
            Console.WriteLine("nothing found");
         }
         
         XmlAttribute versionAttribute = identityNode.Attributes["Version"];
         Console.WriteLine("found version {version}", versionAttribute.Value);

         string targetVersion = settings.Get("AppxVersion");
         if (targetVersion != null)
         {
            targetVersion = ExpressionEngine.Expand(targetVersion, settings);
         }
         if(targetVersion == null)
         {
            Console.WriteLine("property 'AppxVersion' not found");
            return;
         }

         Console.WriteLine("changing to {targetVersion}", targetVersion);
         versionAttribute.Value = targetVersion;

         xmlDoc.Save(path);
      }
   }
}
