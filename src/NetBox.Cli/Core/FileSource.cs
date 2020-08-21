using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Cli.Core
{
   static class FileSource
   {
      private static readonly string LevelUp = ".." + Path.DirectorySeparatorChar;
      private static readonly string LevelUpAlt = ".." + Path.AltDirectorySeparatorChar;

      public static IReadOnlyCollection<string> Search(string pattern, bool recurse)
      {
         string currentDirectory;

         if(Path.IsPathRooted(pattern))
         {
            currentDirectory = Path.GetDirectoryName(pattern);
            pattern = Path.GetFileName(pattern);
         }
         else
         {
            currentDirectory = Environment.CurrentDirectory;
         }

         return Search(currentDirectory, pattern, recurse);
      }

      public static IReadOnlyCollection<string> Search(string currentDirectory, string pattern, bool recurse)
      {
         while(pattern.StartsWith(LevelUp) || pattern.StartsWith(LevelUpAlt))
         {
            Console.WriteLine("raising level");
            currentDirectory = currentDirectory + Path.DirectorySeparatorChar + "..";
            pattern = pattern.Substring(LevelUp.Length);
            Console.WriteLine("current dir: {0}, patten: {1}", currentDirectory, pattern);
         }

         currentDirectory = Path.GetFullPath(currentDirectory);

         PoshWrite($"searching {{{pattern}}} {{(r:{recurse})}}... ", T.HeadingTextColor, ConsoleColor.Gray, T.ActiveTextColor);

         string[] result = Directory.GetFiles(currentDirectory, pattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

         if(result.Length == 0)
         {
            WriteLine("nothing found!", T.ErrorTextColor);
         }
         else
         {
            Write("found ");
            Write(result.Length.ToString(), T.HeadingTextColor);
            WriteLine(" file(s).");
         }

         return result.Select(f => Path.GetFullPath(f)).ToList();
      }
   }
}
