using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetBox.FileFormats
{
   /// <summary>
   /// Reads data from a CSV file
   /// </summary>
   public class CsvReader
   {
      private readonly StreamReader _reader;
      private char[] _buffer;
      private const int BufferSize = 1024 + 10;
      private int _pos;
      private int _size = -1;

      /// <summary>
      /// Creates an instance from an open stream and encoding
      /// </summary>
      public CsvReader(Stream stream, Encoding encoding)
      {
         _reader = new StreamReader(stream, encoding);
      }

      /// <summary>
      /// Reads next row of data.
      /// </summary>
      /// <returns>Null when end of file is reached</returns>
      public IEnumerable<string> ReadNextRow()
      {
         var chars = new List<char>();

         while(ReadNextValue(chars))
         {
            yield return new string(chars.ToArray());

            chars.Clear();
         }

         yield break;
      }

      private bool ReadNextValue(ICollection<char> chars)
      {
         int curr, next;
         bool quoted = false;
         short state = 0;
         while(NextChars(out curr, out next))
         {
            switch(state)
            {
               case 0:
                  if(curr == CsvFormat.ValueQuote)
                  {
                     quoted = true;
                  }
                  break;
            }

         }

         return false;
      }

      private bool NextChars(out int curr, out int next)
      {
         if (_pos >= _size)
         {
            NextBlock();

            if(_size == 0)
            {
               curr = next = -1;
               return false;
            }
         }
         curr = _buffer[_pos++];
        

         if(_pos >= _size)
         {
            NextBlock();

            if(_size == 0)
            {
               next = -1;
               return true;
            }
         }
         next = _buffer[_pos];
         return true;
      }

      private bool NextBlock()
      {
         _size = _reader.ReadBlock(_buffer, 0, BufferSize);
         _pos = 0;
         return _size > 0;
      }
   }
}
