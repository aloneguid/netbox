using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Model
{
   /// <summary>
   /// Options saying how to limit the file search
   /// </summary>
   public class FileSearchOptions
   {
      private readonly List<string> _includePatterns = new List<string>();

      /// <summary>
      /// Craetes a new instance of <see cref="FileSearchOptions"/>
      /// </summary>
      /// <param name="includePattern"></param>
      public FileSearchOptions(string includePattern)
      {
         _includePatterns.Add(includePattern);
      }

      /// <summary>
      /// Include patterns
      /// </summary>
      public ICollection<string> IncludePatterns => _includePatterns;
   }
}
