using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.Themes
{
   public class TerminalTheme
   {
      public ConsoleColor NormalTextColor { get; set; }

      public ConsoleColor ActiveTextColor { get; set; }

      public ConsoleColor InactiveTextColor { get; set; }

      public ConsoleColor HeadingTextColor { get; set; }

      public ConsoleColor SeparatorColor { get; set; }

      public ConsoleColor ErrorTextColor { get; set; }

      public ConsoleColor WarningTextColor { get; set; }

      public static TerminalTheme Default = new TerminalTheme
      {
         NormalTextColor = ConsoleColor.Gray,
         ActiveTextColor = ConsoleColor.Green,
         InactiveTextColor = ConsoleColor.DarkGray,
         HeadingTextColor = ConsoleColor.Yellow,
         ErrorTextColor = ConsoleColor.Red,
         WarningTextColor = ConsoleColor.Yellow,
         SeparatorColor = ConsoleColor.Yellow
      };
   }
}