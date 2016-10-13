using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBox.Application.HttpUtility
{
   internal struct Token
   {
      public int StartIndex;
      public int EndIndex;
      public string Text;
      public TokenType Type;
   }
}
