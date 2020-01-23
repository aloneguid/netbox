using System.Collections.Generic;
using LogMagic;
using NetBox.Cli.Core.Commands.Authoring;
using NetBox.Terminal.Widgets;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Cli.Core.Commands
{
   class AuthoringCommand
   {
      private static readonly ILog log = L.G(typeof(AuthoringCommand));

      public void Execute(IReadOnlyCollection<string> input, ISettings settings)
      {
         foreach (string filePath in input)
         {
            Patch(filePath, settings);
         }
      }

      private void Patch(string filePath, ISettings settings)
      {
         using (var msg = new ProgressMessage("patching " + filePath))
         {
            IFileAuthoring authoring = FileAuthoringFactory.CreateFileAuthoring(filePath, settings);

            if (authoring == null)
            {
               msg.Fail("file format is not supported: " + filePath);
               return;
            }

            WriteLine($"detected as '{authoring.TypeName}'");

            authoring.Update(filePath, settings);
         }
      }
   }
}