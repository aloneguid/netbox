using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetBox.FileFormats
{
   /// <summary>
   /// Reads data from a CSV file. Fast and reliable.
   /// </summary>
   public class CsvReader
   {
      private readonly StreamReader _reader;
      private char[] _buffer;
      private const int BufferSize = 1024 * 10; //10k buffer
      private int _pos;
      private int _size = -1;
      private readonly List<char> _chars = new List<char>();
      private readonly List<string> _row = new List<string>();

      /// <summary>
      /// Creates an instance from an open stream and encoding
      /// </summary>
      public CsvReader(Stream stream, Encoding encoding)
      {
         _reader = new StreamReader(stream, encoding);
         _buffer = new char[BufferSize];
      }

      /// <summary>
      /// Reads next row of data if available.
      /// </summary>
      /// <returns>Null when end of file is reached, or array of strings for each column.</returns>
      public string[] ReadNextRow()
      {
         _row.Clear();
         _chars.Clear();
         bool firstFailed = true;

         while(ReadNextValue())
         {
            firstFailed = false;
            _row.Add(Str());
            _chars.Clear();
         }

         if (firstFailed) return null;

         _row.Add(Str());
         return _row.ToArray();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private string Str()
      {
         return new string(_chars.ToArray());
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private bool ReadNextValue()
      {
         int curr, next;
         bool quoted = false;
         short state = 0;
         while(NextChars(out curr, out next))
         {
            switch(state)
            {
               case 0:  //value start
                  if(curr == CsvFormat.ValueQuote)
                  {
                     //if the value starts with quote it:
                     // - ends with quote
                     // - double quote must be transformed into single quote
                     // - column separator (usuallly ',') can be contained within the value
                     // - line separator '\r' can be inside the value and must be transforted to a proper line feed
                     quoted = true;
                  }
                  else if(curr != '\n')
                  {
                     //if the value doesn't start with quote:
                     // - it can't contain column separator or quote characters inside
                     // - it can't contain line separators
                     _chars.Add((char)curr);
                  }
                  state = 1;
                  break;

               case 1:  //reading value
                  if(quoted)
                  {
                     switch(curr)
                     {
                        case CsvFormat.ValueQuote:
                           if(next == CsvFormat.ValueQuote)
                           {
                              //escaped quote, make a single one
                              _chars.Add(CsvFormat.ValueQuote);

                              //fast-forward to the next character
                              _pos++;
                           }
                           else if(next == CsvFormat.ValueSeparator || next == '\r' || next == '\n')
                           {
                              //this is the end of value
                              state = 2;
                           }
                           else
                           {
                              throw new IOException($"unexpected character {next} after {curr} at position {_pos}");
                           }
                           break;
                        case '\r':
                           _chars.Add('\r');
                           _chars.Add('\n');
                           break;
                        default:
                           _chars.Add((char)curr);
                           break;
                     }
                  }
                  else
                  {
                     _chars.Add((char)curr);

                     //simple and most common case
                     if (next == CsvFormat.ValueSeparator || next == '\r' || next == '\n')
                     {
                        state = 2;
                     }
                  }
                  break;

               case 2:  //end of value
                  //if this character
                  return curr == CsvFormat.ValueSeparator;
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
