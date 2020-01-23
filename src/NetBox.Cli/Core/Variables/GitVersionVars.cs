using System;
using System.Collections.Generic;
using System.Text;
using GitVersion;
using GitVersion.Helpers;
using GitTools;
using LogMagic;

namespace Housework.Core.Variables
{
   static class GitVersionVars
   {
      private static readonly ILog log = L.G(typeof(GitVersionVars));

      private static VersionVariables _vv;

      public static VersionVariables VV
      {
         get
         {
            if(_vv == null)
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
         log.Trace("GitVersion: " + s);
      }

      private static void LogError(string s)
      {
         log.Trace("GitVersion error: " + s);
      }
   }
}
