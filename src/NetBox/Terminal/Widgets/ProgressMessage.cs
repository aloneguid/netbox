using System;
using NetBox.Terminal.Core;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.Widgets
{
   public class ProgressMessage : IDisposable
   {
      private readonly CursorBookmark _fcb = new CursorBookmark();
      private string _message;
      private bool _success = true;

      public ProgressMessage(string message)
      {
         WriteLine(message);
         _message = message;
      }

      public void Fail(string message)
      {
         _message = message;
         _success = false;
      }

      public string Message
      {
         get => _message;
         set
         {
            _message = value;

            using (new CursorLock())
            {
               _fcb.GoTo();
               Write(_message, ConsoleColor.White);
               WriteLine();
            }
         }
      }

      public void Dispose()
      {
         using (new CursorLock())
         {
            _fcb.GoTo();

            Write("[", ConsoleColor.White);
            if (_success)
            {
               Write(" OK ", ConsoleColor.Green);
            }
            else
            {
               Write("FAIL", ConsoleColor.Red);
            }

            Write("] ", ConsoleColor.White);

            Write(_message, ConsoleColor.White);
            WriteLine();
         }

      }
   }
}
