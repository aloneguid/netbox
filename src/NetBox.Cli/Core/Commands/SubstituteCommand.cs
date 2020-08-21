using System.Collections.Generic;
using System.IO;

namespace NetBox.Cli.Core.Commands
{
   class SubstituteCommand
   {
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
         string content = File.ReadAllText(filePath);

         content = ExpressionEngine.Expand(content, settings);

         File.WriteAllText(filePath, content);
      }
   }
}