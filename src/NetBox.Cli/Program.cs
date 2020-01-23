using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Config.Net;
using Cpf.App;
using Housework;
using Housework.Core;
using Housework.Core.Commands;
using Housework.Core.Variables;
using LogMagic;
using LogMagic.Enrichers;
using static Cpf.PoshConsole;

namespace DotNetHousework
{
   class Program
   {
      private static readonly ILog log = L.G(typeof(Program));

      static int Main(string[] args)
      {
         var app = new Application(".NET Housework");
         LinePrimitive<bool> verbose = app.SharedOption<bool>("-v|--verbose", "print verbose output", false);

         app.OnBeforeExecuteCommand(cmd =>
         {
            L.Config
               .WriteTo.PoshConsole("{message}{error}", logProperties: false)
               .When.SeverityIsAtLeast(verbose ? LogSeverity.Verbose : LogSeverity.Information);
         });

         app.Command("echo", cmd =>
         {
            cmd.Description = Help.Command_Echo;
            LinePrimitive<string> expression = cmd.Argument<string>("expression", "expression to execute").Required();
            LinePrimitive<string> settings = Settings(cmd);

            cmd.OnExecute(() =>
            {
               Write(VariablesEngine.Expand(expression, Settings(settings)));
            });
         });

         app.Command("setbuildnumber", cmd =>
         {
            cmd.Description = Help.Command_setbuildnumber;
            LinePrimitive<string> number = cmd.Argument<string>("number", "number to set").Required();
            LinePrimitive<string> settings = Settings(cmd);

            cmd.OnExecute(() =>
            {
               new SetBuildNumberCommand().Execute(number, Settings(settings));
            });
         });

         app.Command("author", cmd =>
         {
            cmd.Description = Help.Command_author;
            LinePrimitive<string> input = cmd.Argument<string>("input", Help.Argument_input).Required();
            LinePrimitive<string> settings = Settings(cmd);
            LinePrimitive<bool> recurse = cmd.Option<bool>("-r|--recurse", Help.Option_recurse, true);

            cmd.OnExecute(() =>
            {
               new AuthoringCommand().Execute(
                  FileSource.Search(input, recurse),
                  Settings(settings));
            });

         });


         app.Command("substitute", cmd =>
         {
            cmd.Description = Help.Command_author;
            LinePrimitive<string> input = cmd.Argument<string>("input", Help.Argument_input).Required();
            LinePrimitive<string> settings = Settings(cmd);
            LinePrimitive<bool> recurse = cmd.Option<bool>("-r|--recurse", Help.Option_recurse, true);

            cmd.OnExecute(() =>
            {
               new SubstituteCommand().Execute(
                  FileSource.Search(input, recurse),
                  Settings(settings));
            });

         });

         app.Command("vars", cmd =>
         {
            cmd.Description = Help.Command_vars;

            LinePrimitive<string> settings = Settings(cmd);

            cmd.OnExecute(() => new VarsCommand().Execute(Settings(settings)));
         });

         app.Command("pushvars", cmd =>
         {
            cmd.Description = Help.Command_pushvars;

            LinePrimitive<string> names = cmd.Argument<string>("names", "list of variable names, comma separated").Required();
            LinePrimitive<string> settings = Settings(cmd);

            cmd.OnExecute(() => new PushVarsCommand().Execute(names, Settings(settings)));
         });

         return app.Execute();
      }

      public static LinePrimitive<string> Settings(Command cmd)
      {
         return cmd.Option<string>("-s|--settings", "optional settings file");
      }

      public static ISettings Settings(LinePrimitive<string> settings)
      {
         var builder = new ConfigurationBuilder<ISettings>();

         if (settings.Value != null)
         {
            string path = Path.GetFullPath(settings);

            if (!File.Exists(path))
            {
               throw new ArgumentException($"settings file does not exist at {path}");
            }

            builder.UseIniFile(path);
         }
         else
         {
            log.Trace("no settings file passed");
         }

         builder.UseEnvironmentVariables();

         return builder.Build();
      }
   }
}