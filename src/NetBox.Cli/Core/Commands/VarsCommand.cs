using System;
using System.Collections.Generic;
using System.Text;
using Housework.Core.Variables;
using LogMagic;

namespace Housework.Core.Commands
{
   class VarsCommand
   {
      private static readonly ILog log = L.G(typeof(VarsCommand));

      public void Execute(ISettings settings)
      {
         log.Info("{0} variables are supported:", VariablesEngine.SupportedVariableNames.Count);

         foreach(string variableName in VariablesEngine.SupportedVariableNames)
         {
            log.Info("  {0}: {1}", variableName, VariablesEngine.Expand($"%{variableName}%"));
         }
      }
   }
}
