using NetBox.Cli.Core.CI;
using NetBox.Cli.Core.Variables;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Cli.Core.Commands
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