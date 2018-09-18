using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal
{
   /// <summary>
   /// Posh functionality for a console
   /// </summary>
   public class PoshConsole
   {
      private readonly ConsoleColor _defaultForeground;

      /// <summary>
      /// Creates an instance of posh console and makes sure none of the colors are affected in the original console.
      /// </summary>
      public PoshConsole()
      {
         _defaultForeground = Console.ForegroundColor;
      }

      /// <summary>
      /// Writes a string in specific color
      /// </summary>
      /// <param name="s"></param>
      /// <param name="color"></param>
      public void Write(string s, ConsoleColor color)
      {
         Console.ForegroundColor = color;
         Console.Write(s);
         Console.ForegroundColor = _defaultForeground;
      }

      /// <summary>
      /// Ask user input
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public string AskInput(string fieldName)
      {
         Write(fieldName, ConsoleColor.Green);
         Write(": ", ConsoleColor.Red);

         return Console.ReadLine();
      }

      /// <summary>
      /// Ask user input without displaying characters. Asterisks are still displayed.
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public string AskPasswordInput(string fieldName)
      {
         Write(fieldName, ConsoleColor.Green);
         Write(": ", ConsoleColor.Red);

         ConsoleKeyInfo key;
         var chars = new List<char>();
         while((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
         {
            if (key.Key == ConsoleKey.Backspace && chars.Count > 0)
            {
               chars.RemoveAt(chars.Count - 1);
               Console.Write("\b \b");
            }
            else
            {
               Console.Write("*");
               chars.Add(key.KeyChar);
            }
         }
         Console.WriteLine();

         return new string(chars.ToArray());
      }
   }
}