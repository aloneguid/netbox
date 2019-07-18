using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.App
{
   class ArgValidationException : ArgumentException
   {
      private readonly string _message;

      public ArgValidationException(string message, string parameterName) : base(message, parameterName)
      {
         _message = message;
      }

      public string OriginalMessage => _message;
   }
}
