using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using LogMagic;
using NetBox.Cli.Core.Variables;

namespace NetBox.Cli.Core.Commands.Authoring
{
   class AppxManifestAuthoring : IFileAuthoring
   {
      private static readonly ILog log = L.G(typeof(DotNetCoreCsProjAuthoring));

      public string TypeName => "appx manifest";

      public void Update(string path, ISettings settings)
      {
         log.Trace("loading {path}", path);
         var xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(File.ReadAllText(path));

         log.Trace("searching for Package/Identity...");

         //ignore namespaces in xml
         XmlNode identityNode = xmlDoc.SelectSingleNode("*[local-name()='Package']/*[local-name()='Identity']");
         if(identityNode == null)
         {
            log.Debug("nothing found");
         }
         
         XmlAttribute versionAttribute = identityNode.Attributes["Version"];
         log.Trace("found version {version}", versionAttribute.Value);

         string targetVersion = settings.Get("AppxVersion");
         if (targetVersion != null)
         {
            targetVersion = VariablesEngine.Expand(targetVersion, settings);
         }
         if(targetVersion == null)
         {
            log.Trace("property 'AppxVersion' not found");
            return;
         }

         log.Trace("changing to {targetVersion}", targetVersion);
         versionAttribute.Value = targetVersion;

         xmlDoc.Save(path);
      }
   }
}
