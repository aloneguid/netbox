using System;
using System.Threading;
using NetBox.Terminal.App;
using NetBox.Terminal.Widgets;
using static NetBox.Terminal.PoshConsole;

namespace Concord.Runner
{
   class Program
   {
      static int Main(string[] args)
      {
         var app = new Application("CPF Demo Util");
         var samples = new Samples();

         LinePrimitive<bool> debug = app.SharedOption<bool>("-d|--debug", "debug?", false);

         app.OnBeforeExecuteCommand(cmd =>
         {
            PoshWriteLine("{exe}{cuting}, {debug}: " + debug.Value, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow);
         });

         app.Command("message", cmd =>
         {
            cmd.OnExecute(() =>
            {
               using (var pb = new ProgressMessage("test"))
               {
                  for(int i = 0; i < 5; i++)
                  {
                     pb.Message = $"updated {i} times";

                     Thread.Sleep(TimeSpan.FromSeconds(1));
                  }
               }
            });
         });

         app.Command("table", cmd =>
         {
            cmd.OnExecute(() =>
            {
               var t = new Table("#", "Description", "Weight");
               t.AddRow(1, "the first row", 45.6);
               t.AddRow(2, "the second", 1.0);
               t.Render(false, 5, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue);
            });
         });

         app.Command("progress-bar", cmd =>
         {
            cmd.Description = "Provides a demo of the built-in progress bar widget, animated and in full shine. This will demo all the variations of progress bar controls and their functions, please be amazed!";
            cmd.OnExecute(() => samples.ProgressBar());
         });

         app.OnError((cmd, ex) =>
         {
            PoshWriteLine($"captured error from command {{{cmd.Name}}}: {{{ex}}}", ConsoleColor.Green, ConsoleColor.Red);

            return true;
         });

         app.Command("simple-args", cmd =>
         {
            cmd.Description = "simple argument parsing";
            LinePrimitive<string> strArg = cmd.Argument<string>("str", "demonstrates a simple string argument");
            LinePrimitive<int> intArg = cmd.Argument<int>("int", "demoes an integer argument that is parsed out from the string").Required();
            LinePrimitive<bool> verbose = cmd.Option<bool>("-c|--cerbose", "when set output is very very very verbose....");
            LinePrimitive<int> intOpt = cmd.Option<int>("-m", "max", 5);
            LinePrimitive<string> defTest = cmd.Option<string>("--default", "test", "...");
            cmd.OnExecute(() =>
            {
               WriteLine("str is " + strArg.Value);
               WriteLine("int is " + intArg.Value);
               WriteLine("cerbose: " + verbose.Value);
               WriteLine("debug: " + debug.Value);
               WriteLine("max: " + intOpt.Value);
               WriteLine("default: " + defTest.Value);
            });
         });

         app.Command("ask-input", cmd =>
         {
            cmd.Description = "Demo on asking input from the user";
            cmd.OnExecute(() =>
            {
               string u = AskInput("username");
               string p = AskPasswordInput("password");
               string pi = AskInput("withprompt", "leave blank to keep default");

               WriteLine("username: " + u);
               WriteLine("password: " + p);
            });
         });

         app.Command("clear", cmd =>
         {
            cmd.Description = "simply clears the screen";
            cmd.OnExecute(() =>
            {
               Console.Clear();
               WriteLine("console cleared!");
            });
         });

         app.Command("supertext", cmd =>
         {
            cmd.Description = "demonstrates posh write formatting";
            cmd.OnExecute(() =>
            {
               PoshWrite("{p}{a}{r}{q} starts with {red} and other {3} letters",
                  ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Yellow);
            });
         });

         app.Command("intercept-error", cmd =>
         {
            cmd.Description = "demonstrates error interceptor";
            cmd.OnExecute(() =>
            {
               throw new IndexOutOfRangeException();
            });
         });

         int exitCode = app.Execute();

         Console.ReadKey();

         return exitCode;
      }
   }
}
