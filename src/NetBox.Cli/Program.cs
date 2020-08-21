using System;
using System.IO;
using Config.Net;
using NetBox.Cli.Core;
using NetBox.Cli.Core.Commands;
using NetBox.Terminal.App;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Cli
{
   class Program
   {
      static int Main(string[] args)
      {
         var app = new NetBox.Terminal.App.Application("NetBox CLI");
         LinePrimitive<bool> verbose = app.SharedOption<bool>("-v|--verbose", "print verbose output", false);

         app.OnBeforeExecuteCommand(cmd =>
         {
            G.Verbose = verbose;
         });

         app.Command("echo", cmd =>
         {
            cmd.Description = Help.Command_Echo;
            LinePrimitive<string> expression = cmd.Argument<string>("expression", "expression to execute").Required();
            LinePrimitive<string> settings = Settings(cmd);

            cmd.OnExecute(() =>
            {
               Write(ExpressionEngine.Expand(expression, Settings(settings)));
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
            if(G.Verbose) WriteLine("no settings file passed", T.ErrorTextColor);
         }

         builder.UseEnvironmentVariables();

         return builder.Build();
      }
   }
}