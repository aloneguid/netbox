using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal
{
   class CursorPosition : IDisposable
   {
      private readonly int _left;
      private readonly int _top;

      public CursorPosition(int left, int top)
      {
         _left = Console.CursorLeft;
         _top = Console.CursorTop;

         Console.SetCursorPosition(left, top);
      }

      public void Dispose()
      {
         Console.SetCursorPosition(_left, _top);
      }
   }
}
