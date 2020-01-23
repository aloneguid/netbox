using System;
using LogMagic;

namespace NetBox.Cli.Core.Variables
{
   static class DateCommands
   {
      private static readonly ILog log = L.G(typeof(DateCommands));

      public static string Format(string command, string arguments)
      {
         log.Debug("formatting {0}({1})", command, arguments);

         DateTime dt = DateTime.Now;

         if (string.IsNullOrEmpty(arguments))
         {
            log.Debug("no format string given");
            return dt.ToString();
         }

         log.Debug("using format string {0}", arguments);
         return dt.ToString(arguments);
      }
   }
}
