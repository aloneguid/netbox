using System.Collections.Generic;
using System.IO;
using Housework.Core.Variables;
using LogMagic;

namespace Housework.Core.Commands
{
   class SubstituteCommand
   {
      private static readonly ILog log = L.G(typeof(SubstituteCommand));

      public string Name => "substitute";

      public string Description => "substitutes variables in a file or group of files";

      public void Execute(IReadOnlyCollection<string> input, ISettings settings)
      {
         foreach(string filePath in input)
         {
            Substitute(filePath, settings);
         }
      }

      private void Substitute(string filePath, ISettings settings)
      {
         log.Debug("processing {0}", filePath);

         log.Debug("reading {0}...", filePath);
         string content = File.ReadAllText(filePath);

         log.Debug("substituting...");
         content = VariablesEngine.Expand(content, settings);

         log.Debug("writing...");
         File.WriteAllText(filePath, content);
      }
   }
}