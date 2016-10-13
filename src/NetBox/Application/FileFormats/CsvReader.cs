using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetBox.Application.FileFormats
{
   /// <summary>
   /// Reads data from a CSV file
   /// </summary>
   public class CsvReader
   {
      private readonly StreamReader _reader;

      /// <summary>
      /// Creates an instance from an open stream and encoding
      /// </summary>
      public CsvReader(Stream stream, Encoding encoding)
      {
         _reader = new StreamReader(stream, encoding);
      }

      /// <summary>
      /// Reads next row of data
      /// </summary>
      /// <returns>Null when end of file is reached</returns>
      public IEnumerable<string> ReadNextRow()
      {
         string line = _reader.ReadLine();
         if(line == null) return null;

         string[] parts = line.Split(CsvFormat.ColumnSeparator);

         return parts.Select(CsvFormat.UnescapeValue);
      }
   }
}
