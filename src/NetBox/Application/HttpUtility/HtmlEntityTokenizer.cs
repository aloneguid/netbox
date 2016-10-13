using System;
using System.Text;

namespace NetBox.Application.HttpUtility
{
   // Breaks up a HTML string into content and entity tokens.
   //
   // HTML Entity Grammer
   // 
   // html:
   //    (content | entity)[0..N]
   //      
   // content:
   //    char (char[1..N except entity-start-char)
   //
   // entity:
   //    entity-start-char (text-entity | numeric-entity) entity-end-char
   //
   // entity-start-char
   //    &
   //
   // entity-end-char
   //    ;
   //
   // text-entity:
   //    char[1..N] except entity-end-char
   //
   // numeric-entity:
   //    numeric-entity-start-char (hex-entity | decimal-entity)
   //
   // numeric-entity-start-char:
   //    #
   //    
   // hex-entity:
   //    hex-entity-start-char hex-digit[1..N]
   //
   // hex-entity-start-char:
   //    x | X 
   //
   // decimal-entity:
   //    decimal-digit[1..N]
   //
   // hex-digit:
   //    0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | A | B | C | D | E | F | a | b | c | d | e | f
   //
   // decimal-digit:
   //    0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
   //
   // char:
   //    any unicode character
   //
   class HtmlEntityTokenizer
   {
      private readonly string _html;
      private readonly StringBuilder _text = new StringBuilder();
      private int _index = -1;
      private TokenType _type;

      public HtmlEntityTokenizer(string html)
      {
         _html = html;
      }

      public Token? Next()
      {
         _text.Clear();

         int startIndex = _index;

         if(!ParseNext())
            return null;

         Token token = new Token();
         token.Text = _text.ToString();
         token.StartIndex = startIndex + 1;
         token.EndIndex = _index + 1;
         token.Type = _type;

         return token;
      }

      private bool ParseNext()
      {
         if(TryParseEntity())
         {
            return true;
         }

         if(TryParseContent())
         {
            return true;
         }

         return false;
      }

      private bool TryParseContent()
      {
         // Content can start with a '&' because it means that 
         // TryParseEntity determined that it wasn't an entity
         if(Read())
         {
            ReadWhile(c => c != HtmlEncodingServices.EntityStartChar);
            _type = TokenType.Content;
            return true;
         }

         return false;
      }

      private bool TryParseEntity()
      {
         if(Peek() != HtmlEncodingServices.EntityStartChar)
            return false;

         int? entityEndIndex = FindEndEntity();
         if(entityEndIndex == null)
            return false; // Not an entity, doesn't end with ';'

         // Skip over '&'
         Skip();

         if(!TryParseNumericEntity())
         {
            ParseTextEntity();
         }

         // Skip over ';'
         Skip();
         return true;
      }

      private void ParseTextEntity()
      {
         ReadWhile(c => c != HtmlEncodingServices.EntityEndChar);
         _type = TokenType.TextEntity;
      }

      private bool ReadEntityBody()
      {
         return ReadWhile(c => !IsEntityEnding(c));
      }

      private bool TryParseNumericEntity()
      {
         if(SkipIf('#'))
         {
            if(TryParseHexEntity())
               return true;

            ParseDecimalEntity();
            return true;
         }

         return false;
      }

      private bool TryParseHexEntity()
      {
         if(SkipIf(HtmlEncodingServices.HexEntityStartChar1) || SkipIf(HtmlEncodingServices.HexEntityStartChar2))
         {
            ReadEntityBody();
            _type = TokenType.HexEntity;
            return true;
         }

         return false; // Not a hex-entity
      }

      private void ParseDecimalEntity()
      {
         ReadEntityBody();
         _type = TokenType.DecimalEntity;
      }

      private int? FindEndEntity()
      {
         int lookAhead = 2;
         while(true)
         {
            char? c = Peek(lookAhead);
            if(c == null)
               return null;

            // Avoid '&quot&'
            if(c == HtmlEncodingServices.EntityStartChar)
               return null;

            if(c == HtmlEncodingServices.EntityEndChar)
            {
               // Avoid an empty entity such as '&;'
               if(lookAhead == 2)
                  return null;

               return _index + lookAhead;
            }

            lookAhead++;
         }
      }

      private bool ReadWhile(Func<char, bool> predicate)
      {
         char? c = Peek();
         if(c == null || !predicate(c.Value))
            return false;

         do
         {
            Read();

            c = Peek();
         } while(c != null && predicate(c.Value));

         return true;
      }

      private char? Peek(int lookAhead = 1)
      {
         int index = _index + lookAhead;

         if(index >= _html.Length)
            return null;

         return _html[index];
      }

      private bool Read()
      {
         char? c = Peek();

         if(c == null)
            return false;

         _text.Append(c.Value);

         Skip();
         return true;
      }

      private bool SkipIf(char symbol)
      {
         if(Peek() != symbol)
            return false;

         Skip();
         return true;
      }

      private void Skip()
      {
         _index++;
      }

      private static bool IsEntityEnding(char c)
      {
         return c == HtmlEncodingServices.EntityEndChar || c == HtmlEncodingServices.EntityStartChar;
      }
   }
}