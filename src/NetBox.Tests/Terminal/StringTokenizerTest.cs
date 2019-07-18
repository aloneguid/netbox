using System;
using System.Collections.Generic;
using NetBox.Terminal.Core;
using Xunit;

namespace Concord.Test
{
   public class StringTokenizerTest
   {
      private readonly StringTokenizer _tokenizer = new StringTokenizer();

      [Fact]
      public void Smoke()
      {
         List<Token> tokens = _tokenizer.Tokenise("{this} is a {funny} number {0:D2}");
      }
   }
}
