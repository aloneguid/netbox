using System;
using System.Collections.Generic;
using System.Text;
using GitVersion;
using GitVersion.Helpers;
using NetBox.Extensions;
using Nustache.Core;

namespace NetBox.Cli.Core
{
   class ExpressionEngine
   {
      private static Dictionary<string, Func<string>> _gitVersionNames = new Dictionary<string, Func<string>>(StringComparer.InvariantCultureIgnoreCase)
      {
         ["major"] = () => GitVersionVars.Major,
         ["minor"] = () => GitVersionVars.Minor,
         ["patch"] = () => GitVersionVars.Patch,
         ["preReleaseTag"] = () => GitVersionVars.PreReleaseTag,
         ["semVer"] = () => GitVersionVars.SemVer,
         ["legacySemVer"] = () => GitVersionVars.LegacySemVer,
         ["assemblySemVer"] = () => GitVersionVars.AssemblySemVer
      };

      static ExpressionEngine()
      {
         Helpers.Register("date", FormatDateTime);
         Helpers.Register("gitVersion", GitVersion);
      }

      public static string Expand(string template, ISettings settings)
      {
         string result = Render.StringToString(template, new Dictionary);

         return result;
      }

      static void FormatDateTime(RenderContext context, IList<object> arguments, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
      {
         DateTime dateTime = DateTime.UtcNow;

         if (options != null && options.ContainsKey("format"))
            context.Write(dateTime.ToString(options["format"] as string));
         else
            context.Write(dateTime.ToString());
      }

      static void GitVersion(RenderContext context, IList<object> arguments, IDictionary<string, object> options, RenderBlock fn, RenderBlock inverse)
      {
         if(arguments.Count > 0 && arguments[0] is string varName)
         {
            if(_gitVersionNames.TryGetValue(varName, out Func<string> value))
            {
               context.Write(value());
            }
         }
      }

      private static VersionVariables _vv;
      public static VersionVariables GitVersionVars
      {
         get
         {
            if (_vv == null)
            {
               var core = new ExecuteCore(new FileSystem());

               Logger.SetLoggers(Log, Log, Log, LogError);

               if (core.TryGetVersion(Environment.CurrentDirectory, out VersionVariables vv, true, new Authentication()))
               {
                  _vv = vv;
               }
               else
               {
                  throw new ApplicationException("coulld not call GitVersion");
               }
            }

            return _vv;
         }
      }

      private static void Log(string s)
      {
         Console.WriteLine("GitVersion: " + s);
      }

      private static void LogError(string s)
      {
         Console.WriteLine("GitVersion error: " + s);
      }
   }
}
