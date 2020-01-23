using System;
using static Cpf.PoshConsole;

namespace Housework.Core.CI
{
   static class AzurePipelines
   {
      public static void UpdateBuildNumber(string number)
      {
         Write("##vso[build.updatebuildnumber]", ConsoleColor.DarkGray);
         Write(number, ConsoleColor.DarkGray);
         WriteLine();
      }

      public static void SetVariable(string name, string value)
      {
         //"##vso[task.setvariable variable=testvar;]testvalue"
         Write("##vso[task.setvariable variable=", ConsoleColor.DarkGray);
         Write(name, ConsoleColor.DarkGray);
         Write(";]");
         Write(value, ConsoleColor.DarkGray);
         WriteLine();
      }
   }
}
