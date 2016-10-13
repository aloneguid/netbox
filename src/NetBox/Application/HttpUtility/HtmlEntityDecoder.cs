using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace NetBox.Application.HttpUtility
{
   internal class HtmlEntityDecoder
   {
      private readonly string _html;
      private readonly IDictionary<string, char> _entityToCharMap;

      public HtmlEntityDecoder(string html, IDictionary<string, char> entityToCharMap)
      {
         Debug.Assert(html != null);
         Debug.Assert(entityToCharMap != null);

         _html = html;
         _entityToCharMap = entityToCharMap;
      }

      public void Decode(TextWriter writer)
      {
         HtmlEntityTokenizer tokenizer = new HtmlEntityTokenizer(_html);

         Token? token;
         while((token = tokenizer.Next()) != null)
         {
            WriteToken(token.Value, writer);
         }
      }

      private void WriteToken(Token token, TextWriter writer)
      {
         switch(token.Type)
         {
            case TokenType.Content:
               WriteContent(token.Text, writer);
               break;

            case TokenType.TextEntity:
               WriteTextEntity(token, writer);
               break;

            case TokenType.DecimalEntity:
               WriteDecimalEntity(token, writer);
               break;

            case TokenType.HexEntity:
               WriteHexEntity(token, writer);
               break;

         }
      }

      private static void WriteContent(string text, TextWriter writer)
      {
         writer.Write(text, writer);
      }

      private void WriteTextEntity(Token token, TextWriter writer)
      {
         char c;
         if(_entityToCharMap.TryGetValue(token.Text, out c))
         {
            // We recognize the entity
            writer.Write(c);
         }
         else
         {
            // Do not recognize, treat it as content
            WriteAsContext(token, writer);
         }
      }

      private void WriteDecimalEntity(Token token, TextWriter writer)
      {
         WriteNumericEntity(token, writer, NumberStyles.Integer);
      }

      private void WriteHexEntity(Token token, TextWriter writer)
      {
         WriteNumericEntity(token, writer, NumberStyles.AllowHexSpecifier);
      }

      private void WriteNumericEntity(Token token, TextWriter writer, NumberStyles styles)
      {
         ushort value;
         if(ushort.TryParse(token.Text, styles, CultureInfo.InvariantCulture, out value))
         {
            writer.Write((char)value);
         }
         else
         {
            // Failed to parse, write it as content
            WriteAsContext(token, writer);
         }
      }

      private void WriteAsContext(Token token, TextWriter writer)
      {
         WriteContent(_html.Substring(token.StartIndex, token.EndIndex - token.StartIndex), writer);
      }
   }
}
