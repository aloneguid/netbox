using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Housework.Console.Core.Variables;
using LogMagic;
using Sprache;

namespace Housework.Core.Variables
{
   /// <summary>
   /// </summary>
   public static class VariablesEngine
   {
      private static readonly ILog log = L.G(typeof(VariablesEngine));

      private static readonly Dictionary<string, Func<string, string, ISettings, string>> variableToFunc =
         new Dictionary<string, Func<string, string, ISettings, string>>(StringComparer.InvariantCultureIgnoreCase)
         {
            ["git.commitCount"] = (c, a, s) => ProcessUtils.ExecAndGetOutput("git", "rev-list --all --count"),
            ["git.branchName"] = (c, a, s) => GetGitBranchName(),
            ["git.nonMasterBranchName"] = (c, a, s) => GetNonMasterGitBranchName(),
            ["git.commitHash"] = (c, a, s) => ProcessUtils.ExecAndGetOutput("git", "log --pretty=format:'%h' -n 1"),
            ["git.longCommitHash"] = (c, a, s) => ProcessUtils.ExecAndGetOutput("git", "log --pretty=format:'%H' -n 1"),
            ["date"] = (cmd, arg, _) => DateCommands.Format(cmd, arg),
            ["coalesce"] = (_, arg, settings) => Coalesce(arg, settings),


            //gitVersion integration
            ["gitVersion.major"] = (c, a, s) => GitVersionVars.VV.Major,
            ["gitVersion.minor"] = (c, a, s) => GitVersionVars.VV.Minor,
            ["gitVersion.patch"] = (c, a, s) => GitVersionVars.VV.Patch,
            ["gitVersion.preReleaseTag"] = (c, a, s) => GitVersionVars.VV.PreReleaseTag,
            ["gitVersion.preReleaseLabel"] = (c, a, s) => GitVersionVars.VV.PreReleaseLabel,
            ["gitVersion.preReleaseNumber"] = (c, a, s) => GitVersionVars.VV.PreReleaseNumber,
            ["gitVersion.semVer"] = (c, a, s) => GitVersionVars.VV.SemVer,
            ["gitVersion.legacySemVer"] = (c, a, s) => GitVersionVars.VV.LegacySemVer,
            ["gitVersion.legacySemVerPadded"] = (c, a, s) => GitVersionVars.VV.LegacySemVerPadded
         };

      //Sprache expression grammar

      public static string Expand(string s, ISettings settings = null)
      {
         var cache = new Dictionary<string, string>();

         return Expand(s, cache, settings);
      }

      public static IReadOnlyCollection<string> SupportedVariableNames => new List<string>(variableToFunc.Keys);

      private static string Expand(string s, Dictionary<string, string> varToValueCache, ISettings settings)
      {
         if (!s.Contains(ExpressionParser.Delimiter))
         {
            return s;
         }

         try
         {
            IReadOnlyCollection<VariableDefinition> variables = ExpressionParser.ParseVariables(s);

            foreach (VariableDefinition variable in variables)
            {
               if (!varToValueCache.TryGetValue(variable.Value, out string repTarget))
               {
                  log.Debug("calculating {0}", variable.SearchValue);

                  repTarget = GetWithSettings(variable.Value, settings)
                  ?? GetWithFunc(variable, settings)
                  ?? string.Empty;

                  repTarget = Expand(repTarget, settings);
                  varToValueCache[variable.Value] = repTarget;

                  log.Debug("var {0} => {1}", variable, repTarget);
                  s = s.Replace(variable.SearchValue, repTarget);
               }
            }

         }
         catch(ParseException ex)
         {
            log.Trace("parser exception", ex);
         }

         return s;
      }

      private static string GetWithFunc(VariableDefinition variable, ISettings settings)
      {
         if (!variableToFunc.TryGetValue(variable.Name, out Func<string, string, ISettings, string> fn)) return null;

         string result = fn(variable.Name, variable.Args, settings);

         if(result != null)
         {
            log.Debug("calculated function");
         }

         return result;
      }

      private static string GetWithSettings(string name, ISettings settings)
      {
         if (settings == null)
         {
            return null;
         }

         string value = settings.Get(name);
         if(value != null)
         {
            log.Debug("found in settting", name);
         }
         return value;
      }

      private static string Coalesce(string args, ISettings settings)
      {
         log.Debug("splitting args...");
         string[] parts = args.Split(';', StringSplitOptions.RemoveEmptyEntries);
         log.Debug("found {0} commands", parts.Length);

         foreach(string cmd in parts)
         {
            string unescaped = cmd.Replace(ExpressionParser.Escaped, ExpressionParser.Delimiter);
            log.Debug("executing {0}...", unescaped);
            string result = Expand(unescaped, settings);

            if (!string.IsNullOrEmpty(result))
            {
               log.Trace("found value {0}", result);
               return result;
            }
         }

         log.Trace("no values found");
         return string.Empty;
      }

      #region [ Simple Commands ]

      private static string GetGitBranchName()
      {
         return ProcessUtils.ExecAndGetOutput("git", "branch -q").Trim(' ', '*');
      }

      private static string GetNonMasterGitBranchName()
      {
         string branchName = GetGitBranchName();

         return branchName == "master" ? string.Empty : branchName;
      }

      #endregion
   }
}