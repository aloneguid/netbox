using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Housework.Core
{
   static class ProcessUtils
   {
      public static string ExecAndGetOutput(string fileName, string arguments)
      {
         var psi = new ProcessStartInfo
         {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
         };

         var proc = new Process { StartInfo = psi };

         try
         {
            if (!proc.Start()) return null;
         }
         catch(Exception)
         {
            return null;
         }

         var res = new StringBuilder();

         while(!proc.StandardOutput.EndOfStream)
         {
            string line = proc.StandardOutput.ReadLine();
            res.AppendLine(line);
         }

         proc.WaitForExit();

         return res.ToString().Trim();
      }
   }
}