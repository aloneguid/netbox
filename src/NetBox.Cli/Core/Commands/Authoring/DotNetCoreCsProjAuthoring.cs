using System;
using System.IO;
using System.Xml;

namespace NetBox.Cli.Core.Commands.Authoring
{
   class DotNetCoreCsProjAuthoring : IFileAuthoring
   {
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

         value = ExpressionEngine.Expand(value, _settings);

         Console.WriteLine("{0} => {1}", name, value);

         Console.WriteLine("searching for {0}...", name);
         string xPath = $"Project/PropertyGroup/{name}";

         XmlNodeList nodes = document.SelectNodes(xPath);
         Console.WriteLine("found {0} elements", nodes.Count);

         if(nodes.Count > 0)
         {
            XmlNode node = nodes[0];
            string oldValue = node.InnerText;

            if (oldValue == value)
            {
               Console.WriteLine("value hasn't changed from {0}", value);
            }
            else
            {
               node.InnerText = value;
               Console.WriteLine("{0} => {1}", oldValue, value);
            }
         }
         else
         {
            XmlNodeList pgs = document.SelectNodes("Project/PropertyGroup");
            if(pgs.Count == 0)
            {
               Console.WriteLine("no PropertyGroup elements defined.");
               return;
            }

            Console.WriteLine("creating new node...");
            XmlNode container = pgs[0];
            XmlNode newElement = document.CreateNode(XmlNodeType.Element, name, null);
            newElement.InnerText = value;
            container.AppendChild(newElement);
            Console.WriteLine("node created and value set to {0}", value);
         }

      }
   }
}
