using System;
using System.Collections.Generic;
using System.Text;
using NetBox.Terminal.Core;
using NetBox.Terminal.Themes;

namespace NetBox.Terminal
{
   /// <summary>
   /// Posh functionality for a console
   /// </summary>
   public static class PoshConsole
   {
      private static readonly ConsoleColor DefaultForeground;
      private static readonly object CLock = new object();
      private static readonly TerminalTheme Theme;

      static PoshConsole()
      {
         try
         {
            int left = Console.CursorLeft;
            CanMoveCursor = true;
         }
         catch
         {
            CanMoveCursor = false;
         }

         Theme = TerminalTheme.Default;

         DefaultForeground = Console.ForegroundColor;
      }

      public static bool CanMoveCursor { get; }

      /// <summary>
      /// Color theme
      /// </summary>
      public static TerminalTheme T => Theme;

      /// <summary>
      /// Writes a string in specific color
      /// </summary>
      /// <param name="s"></param>
      /// <param name="color"></param>
      public static void Write(object s, ConsoleColor? color = null)
      {
         lock (CLock)
         {
            Write(s, color, false);
         }
      }

      public static void Write(object s)
      {
         lock(CLock)
         {
            Write(s, DefaultForeground, false);
         }
      }

      /// <summary>
      /// Writes a string in specific color
      /// </summary>
      /// <param name="s"></param>
      /// <param name="color"></param>
      public static void WriteLine(object s, ConsoleColor? color = null)
      {
         lock (CLock)
         {
            Write(s, color, true);
         }
      }

      public static void WriteLine(object s)
      {
         lock (CLock)
         {
            Write(s, DefaultForeground, true);
         }
      }

      private static void Write(object s, ConsoleColor? color, bool newLine)
      {
         Console.ForegroundColor = color == null ? Theme.NormalTextColor : color.Value;
         Console.Write(s);
         Console.ForegroundColor = DefaultForeground;

         if(newLine)
         {
            Console.WriteLine();
         }
      }

      /// <summary>
      /// Adds an empty line
      /// </summary>
      public static void WriteLine()
      {
         lock (CLock)
         {
            Console.WriteLine();
         }
      }

      public static void PoshWriteLine(string format, params object[] parameters)
      {
         PoshWrite(true, format, parameters);
      }

      public static void PoshWrite(string format, params object[] parameters)
      {
         PoshWrite(false, format, parameters);
      }

      private static void PoshWrite(bool newLine, string format, params object[] parameters)
      {
         if (format == null)
            return;

         List<Token> tokens = new StringTokenizer().Tokenise(format);
         lock (CLock)
         {
            foreach (Token token in tokens)
            {
               switch (token.Type)
               {
                  case TokenType.String:
                     Write(token.Value, null, false);
                     break;
                  case TokenType.Parameter:
                     if (parameters == null)
                        continue;

                     object value = null;
                     if(token.Position < parameters.Length)
                     {
                        value = parameters[token.Position];
                     }

                     if(value is ConsoleColor cc)
                     {
                        Write(token.Value, cc, false);
                     }
                     else
                     {
                        Write(token.Value, null, false);
                     }

                     break;
               }
            }

            if(newLine)
            {
               Console.WriteLine();
            }
         }
      }

      internal static void WriteInColumn(int left, int width, string text, bool padFirstLine = true, ConsoleColor? color = null)
      {
         void WriteColumnLine(bool pad, StringBuilder lsb)
         {
            if (pad)
            {
               Write(new string(' ', left));
            }

            WriteLine(lsb.ToString());
            lsb.Clear();
         }

         string[] tps = text.Split(' ');

         var sb = new StringBuilder();
         int lineNo = 0;
         for(int i = 0; i < tps.Length; i++)
         {
            if(sb.Length + 1 + tps[i].Length > width)
            {
               WriteColumnLine(lineNo > 0 || padFirstLine, sb);

               lineNo += 1;
            }
            else
            {
               if (sb.Length > 0)
                  sb.Append(" ");

               sb.Append(tps[i]);
            }
         }

         if(sb.Length > 0)
         {
            WriteColumnLine(lineNo > 0 || padFirstLine, sb);
         }
      }

      /// <summary>
      /// Ask user input
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public static string AskInput(string fieldName, string hint = null)
      {
         lock (CLock)
         {
            return LineInput(fieldName, null, hint);
         }
      }

      /// <summary>
      /// Ask user input without displaying characters. Asterisks are still displayed.
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public static string AskPasswordInput(string fieldName, string hint = null)
      {
         lock (CLock)
         {
            return LineInput(fieldName, '*', hint);
         }
      }

      private static string LineInput(string fieldName, char? replacementChar = null, string hint = null)
      {
         void Clear()
         {
            Console.CursorLeft = 0;
            Write(fieldName, Theme.ActiveTextColor);
            Write(": ", Theme.SeparatorColor);
            Write(new string(' ', Console.WindowWidth - fieldName.Length - 3));
            Console.CursorLeft = fieldName.Length + 2;
         }

         lock (CLock)
         {

            int fieldLength = fieldName.Length + 2;
            bool hintVisible = false;

            Clear();

            if (!string.IsNullOrEmpty(hint))
            {
               Write(hint, T.InactiveTextColor);
               hintVisible = true;
            }

            ConsoleKeyInfo key;
            var chars = new List<char>();
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
               if(hintVisible)
               {
                  Clear();
                  hintVisible = false;
               }

               if (key.Key == ConsoleKey.Backspace)
               {
                  if (chars.Count > 0)
                  {
                     chars.RemoveAt(chars.Count - 1);
                     Write("\b \b");
                  }
               }
               else
               {
                  if (replacementChar != null)
                  {
                     Write(new string(replacementChar.Value, 1));
                  }
                  else
                  {
                     Write(new string(key.KeyChar, 1));
                  }

                  chars.Add(key.KeyChar);
               }
            }

            Console.WriteLine();

            return new string(chars.ToArray());
         }
      }

   }
}