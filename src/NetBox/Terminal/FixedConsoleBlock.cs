using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal
{
   class FixedConsoleBlock
   {
      private readonly int _left;
      private readonly int _top;
      private readonly PoshConsole _pc = new PoshConsole();

      public FixedConsoleBlock()
      {
         _left = Console.CursorLeft;
         _top = Console.CursorTop;
      }

      public PoshConsole Pc => _pc;

      public void Write(Action<PoshConsole> poshAction)
      {
         using (new CursorPosition(_left, _top))
         {
            poshAction(_pc);
         }
      }
   }
}
