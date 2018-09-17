using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal
{
   public class PoshConsole
   {
      private readonly ConsoleColor _defaultForeground;

      public PoshConsole()
      {
         _defaultForeground = Console.ForegroundColor;
      }

      public void Write(string s, ConsoleColor color)
      {
         Console.ForegroundColor = color;
         Console.Write(s);
         Console.ForegroundColor = _defaultForeground;
      }
   }
}