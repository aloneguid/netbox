using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.App
{
   /// <summary>
   /// Common interface for validating a line primitive
   /// </summary>
   public interface ILinePrimitiveValidator
   {
      bool IsValid(string primitiveName, string value, out string message);
   }
}
