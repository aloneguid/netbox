using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal
{
   /// <summary>
   /// Draws a progress bar in the console
   /// </summary>
   public class ConsoleProgressBar : IDisposable
   {
      private readonly bool _hasSubtitle;
      private readonly int _min;
      private readonly int _max;
      private int _value;
      private int _left;
      private int _top;
      private string _subtitle;
      private readonly FixedConsoleBlock _fcb = new FixedConsoleBlock();
      private bool _restore = false;

      /// <summary>
      /// Creates a new instance of the progress bar
      /// </summary>
      /// <param name="hasSubtitle"></param>
      /// <param name="min"></param>
      /// <param name="max"></param>
      public ConsoleProgressBar(bool hasSubtitle, int min = 0, int max = 100)
      {
         _hasSubtitle = hasSubtitle;
         _min = min;
         _max = max;
         _value = min;
         Draw();
         _restore = true;
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
         if (_restore)
            _fcb.Write(Draw);
         else
            Draw(_fcb.Pc);
      }

      private void Draw(PoshConsole pc)
      {
         int value = _value - _min;
         int max = _max - _min;

         double perc = 100.0 * value / max;

         pc.Write("[", ConsoleColor.Green);

         int filled = (int)perc;
         Console.Write(new string('=', filled));
         pc.Write(">", ConsoleColor.Red);
         Console.Write(new string(' ', 100 - filled));
         pc.Write("] ", ConsoleColor.Green);

         Console.Write(perc);
         pc.Write("%   ", ConsoleColor.Yellow);
         Console.WriteLine();

         if(_hasSubtitle)
         {
            pc.Write(_subtitle, ConsoleColor.Gray);
            Console.WriteLine();
         }
         
      }

      /// <summary>
      /// Gracefully shuts down the progress counter
      /// </summary>
      public void Dispose()
      {
         _value = _max;

         Draw();
      }
   }
}