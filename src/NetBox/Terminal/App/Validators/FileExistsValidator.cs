using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetBox.Terminal.App.Validators
{
   class FileExistsValidator : ILinePrimitiveValidator
   {
      public bool IsValid(string primitiveName, string value, out string message)
      {
         string path = Path.GetFullPath(value);

         if(!File.Exists(path))
         {
            message = $"file does not exist at {path}";
            return false;
         }

         message = null;
         return true;
      }
   }
}
