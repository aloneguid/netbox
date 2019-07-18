using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.Core
{
   class CursorBookmark
   {
      private int _left;
      private int _top;

      public CursorBookmark()
      {
         Mark();
      }

      public void Mark()
      {
         if (PoshConsole.CanMoveCursor)
         {
            _left = Console.CursorLeft;
            _top = Console.CursorTop;
         }
      }

      public void GoTo()
      {
         if (PoshConsole.CanMoveCursor)
         {
            Console.SetCursorPosition(_left, _top);
         }
      }
   }
}
