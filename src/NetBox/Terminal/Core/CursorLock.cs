using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.Core
{
   class CursorLock : IDisposable
   {
      private readonly int _left;
      private readonly int _top;

      public CursorLock()
      {
         if (PoshConsole.CanMoveCursor)
         {
            _left = Console.CursorLeft;
            _top = Console.CursorTop;
         }
      }

      public void Dispose()
      {
         if (PoshConsole.CanMoveCursor)
         {
            Console.SetCursorPosition(_left, _top);
         }
      }
   }
}
