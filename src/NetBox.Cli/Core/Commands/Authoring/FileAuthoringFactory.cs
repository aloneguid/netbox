using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetBox.Cli.Core.Commands.Authoring
{
   static class FileAuthoringFactory
   {
      public static IFileAuthoring CreateFileAuthoring(string filePath, ISettings settings)
      {
         //read first block
         string block = File.ReadAllText(filePath);

         return
            CreateNetCoreCsProjAuthoring(block, settings) ??
            CreateAppxManifestAuthoring(block, settings) ??
            null;
      }

      private static IFileAuthoring CreateNetCoreCsProjAuthoring(string header, ISettings settings)
      {
         if (!header.Trim().StartsWith("<Project Sdk=\"Microsoft.NET.Sdk")) return null;

         return new DotNetCoreCsProjAuthoring(settings);
      }

      private static IFileAuthoring CreateAppxManifestAuthoring(string header, ISettings settings)
      {
         if (!header.Contains("http://schemas.microsoft.com/appx/manifest/foundation/windows10")) return null;

         return new AppxManifestAuthoring();
      }
   }
}
