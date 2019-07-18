using System;
using System.Collections.Generic;
using System.Linq;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.App.Help
{
   class ConsoleHelpGenerator : IHelpGenerator
   {
      public ConsoleHelpGenerator()
      {
      }

      public void GenerateHelp(Application app)
      {
         WriteAppHeader(app);

         WriteCommands(app);
      }

      private void WriteCommands(Application app)
      {
         WriteLine("Commands", T.ActiveTextColor);

         int longestCommandName = app.Commands.Select(cmd => cmd.Name.Length).Max();

         foreach(Command cmd in app.Commands)
         {
            string name = cmd.Name.PadRight(longestCommandName);
            Write("  ");
            Write(name, T.HeadingTextColor);

            Write("  ");
            if (cmd.Description != null)
            {
               WriteInColumn(longestCommandName + 4, Console.WindowWidth - longestCommandName - 3, cmd.Description, false);
            }
            else
            {
               WriteLine();
            }
         }
      }

      private void WriteAppHeader(Application app)
      {
         string header = $"{app.Name} v{app.Version}";

         Write(app.Name, T.ActiveTextColor);
         Write(" ");
         Write("v", T.ActiveTextColor);

         bool first = true;
         foreach (string vp in app.Version.Split('.'))
         {
            if (!first)
               Write(".", T.ActiveTextColor);
            Write(vp, T.HeadingTextColor);
            first = false;
         }
         WriteLine();

         /*string line = new string('=', header.Length);
         WriteLine(line, ConsoleColor.DarkGray);
         WriteLine();*/
      }

      public void GenerateHelp(Command cmd)
      {
         Write("Usage: ");
         Write(cmd.Name, T.ActiveTextColor);

         if(cmd.Arguments.Count > 0)
         {
            foreach(LinePrimitive arg in cmd.Arguments)
            {
               Write(" <");
               Write(arg.Name, T.ActiveTextColor);
               Write(">");
            }
         }

         if(cmd.Options.Count > 0)
         {
            Write(" [options]");
         }

         WriteLine();


         Write("  ");
         Write(cmd.Description);
         WriteLine();

         GenerateLinePrimitivesHelp("Arguments", cmd.Arguments);
         GenerateLinePrimitivesHelp("Options", cmd.Options);
      }

      private void GenerateLinePrimitivesHelp(string header, IReadOnlyCollection<LinePrimitive> primitives)
      {
         if (primitives.Count > 0)
         {
            WriteLine();
            WriteLine(header, T.ActiveTextColor);

            int lpn = primitives.Select(a => a.Name.Length).Max();

            foreach (LinePrimitive lp in primitives)
            {
               Write("  ");
               string name = lp.Name.PadRight(lpn);
               Write(name, T.ActiveTextColor);

               Write("  ");
               WriteInColumn(lpn + 4, Console.WindowWidth - lpn - 3, lp.Description, false);
            }
         }
      }

   }
}
