using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Housework.Core.Variables;
using LogMagic;

namespace Housework.Core.Commands.Authoring
{
   class DotNetCoreCsProjAuthoring : IFileAuthoring
   {
      private static readonly ILog log = L.G(typeof(DotNetCoreCsProjAuthoring));
      private readonly ISettings _settings;

      public DotNetCoreCsProjAuthoring(ISettings settings)
      {
         _settings = settings;
      }

      public string TypeName => "msbuild project";

      public void Update(string path, ISettings settings)
      {
         var xmlDoc = new XmlDocument();
         xmlDoc.LoadXml(File.ReadAllText(path));

         UpdateProperty(xmlDoc, "Version", settings.Get("Version"));
         UpdateProperty(xmlDoc, "FileVersion", settings.Get("FileVersion"));
         UpdateProperty(xmlDoc, "AssemblyVersion", settings.Get("AssemblyVersion"));

         UpdateProperty(xmlDoc, "Authors", settings.Get("Authors"));
         UpdateProperty(xmlDoc, "Copyright", settings.Get("Copyright"));
         UpdateProperty(xmlDoc, "PackageIconUrl", settings.Get("PackageIconUrl"));
         UpdateProperty(xmlDoc, "PackageProjectUrl", settings.Get("PackageProjectUrl"));
         UpdateProperty(xmlDoc, "RepositoryUrl", settings.Get("RepositoryUrl"));
         UpdateProperty(xmlDoc, "PackageLicenseUrl", settings.Get("PackageLicenseUrl"));
         UpdateProperty(xmlDoc, "RepositoryType", settings.Get("RepositoryType"));
         UpdateProperty(xmlDoc, "Company", settings.Get("Company"));
         UpdateProperty(xmlDoc, "Tags", settings.Get("Tags"));
         UpdateProperty(xmlDoc, "ReleaseNotes", settings.Get("ReleaseNotes"));

         xmlDoc.Save(path);
      }

      private void UpdateProperty(XmlDocument document, string name, string value)
      {
         if (string.IsNullOrEmpty(value)) return;

         value = VariablesEngine.Expand(value, _settings);

         log.Trace("{0} => {1}", name, value);

         log.Debug("searching for {0}...", name);
         string xPath = $"Project/PropertyGroup/{name}";

         XmlNodeList nodes = document.SelectNodes(xPath);
         log.Debug("found {0} elements", nodes.Count);

         if(nodes.Count > 0)
         {
            XmlNode node = nodes[0];
            string oldValue = node.InnerText;

            if (oldValue == value)
            {
               log.Debug("value hasn't changed from {0}", value);
            }
            else
            {
               node.InnerText = value;
               log.Debug("{0} => {1}", oldValue, value);
            }
         }
         else
         {
            XmlNodeList pgs = document.SelectNodes("Project/PropertyGroup");
            if(pgs.Count == 0)
            {
               log.Debug("no PropertyGroup elements defined.");
               return;
            }

            log.Debug("creating new node...");
            XmlNode container = pgs[0];
            XmlNode newElement = document.CreateNode(XmlNodeType.Element, name, null);
            newElement.InnerText = value;
            container.AppendChild(newElement);
            log.Debug("node created and value set to {0}", value);
         }

      }
   }
}
