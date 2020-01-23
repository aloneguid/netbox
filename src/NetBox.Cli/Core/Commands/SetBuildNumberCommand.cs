using System;
using Cpf.Widgets;
using Housework.Core.CI;
using Housework.Core.Variables;
using static Cpf.PoshConsole;

namespace Housework.Core.Commands
{
   /// <summary>
   /// Sets current Continuous Integration build number using all known methods so far.
   /// </summary>
   class SetBuildNumberCommand
   {
       public void Execute(string targetNumber, ISettings settings)
      {
         targetNumber = VariablesEngine.Expand(targetNumber, settings);
         Write("build number is ");
         WriteLine(targetNumber, T.HeadingTextColor);

         AzurePipelines.UpdateBuildNumber(targetNumber);
         ProcessUtils.ExecAndGetOutput("appveyor", $"UpdateBuild -Version {targetNumber}");
      }
   }
}