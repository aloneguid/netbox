using System;
using NetBox.Cli.Console.Core.Variables;
using NetBox.Cli.Core.CI;
using NetBox.Cli.Core.Variables;

namespace NetBox.Cli.Core.Commands
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
