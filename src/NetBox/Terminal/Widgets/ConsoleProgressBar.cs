using System;
using System.Collections.Generic;
using System.Text;
using NetBox.Terminal.Core;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.Widgets
{
   /// <summary>
   /// Draws a progress bar in the console
   /// </summary>
   public class ProgressBar : IDisposable
   {
      private readonly bool _hasSubtitle;
      private readonly int _min;
      private int _max;
      private int _value;
      private string _subtitle;
      private readonly CursorBookmark _fcb = new CursorBookmark();

      /// <summary>
      /// Creates a new instance of the progress bar
      /// </summary>
      /// <param name="hasSubtitle"></param>
      /// <param name="min"></param>
      /// <param name="max"></param>
      public ProgressBar(bool hasSubtitle, int min = 0, int max = 100)
      {
         _hasSubtitle = hasSubtitle;
         _min = min;
         _max = max;
         _value = min;
         Draw();
      }

      public int Value
      {
         get => _value;
         set
         {
            _value = value;
            Draw();
         }
      }

      public int Max
      {
         get => _max;
         set
         {
            _max = value;
            Draw();
         }
      }

      public string Subtitle
      {
         get => _subtitle;
         set
         {
            _subtitle = value;
            Draw();
         }
      }

      private void Draw()
      {
         int value = _value - _min;
         int max = _max - _min;

         double perc = 100.0 * value / max;

         if (CanMoveCursor)
         {
            _fcb.GoTo();

            Write("[", ConsoleColor.Green);

            int filled = (int)perc;
            Write(new string('=', filled));
            Write(">", ConsoleColor.Red);
            Write(new string(' ', 100 - filled));
            Write("] ", ConsoleColor.Green);

            Write(perc.ToString());
            Write("%   ", ConsoleColor.Yellow);
            WriteLine();

            if (_hasSubtitle)
            {
               Write(_subtitle, ConsoleColor.Gray);
               WriteLine();
            }
         }
         else
         {
            Write(perc.ToString(), T.ActiveTextColor);
            Write(",", T.NormalTextColor);
         }
      }

      /// <summary>
      /// Gracefully shuts down the progress counter
      /// </summary>
      public void Dispose()
      {
         _value = _max;

         Draw();

         if(!CanMoveCursor)
         {
            WriteLine();
         }
      }
   }
}