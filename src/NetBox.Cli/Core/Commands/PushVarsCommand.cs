using System;
using System.Collections.Generic;
using System.Text;
using Housework.Console.Core.Variables;
using Housework.Core.CI;
using Housework.Core.Variables;

namespace Housework.Core.Commands
{
   class PushVarsCommand
   {
      public void Execute(string commaSeparatedVariables, ISettings settings)
      {
         foreach(string rawName in commaSeparatedVariables.Split(',', StringSplitOptions.RemoveEmptyEntries))
         {
            string name = rawName.Trim();
            string var = $"{ExpressionParser.Delimiter}{name}{ExpressionParser.Delimiter}";
            string value = VariablesEngine.Expand(var, settings);

            AzurePipelines.SetVariable(name, value);
         }
      }
   }
}
