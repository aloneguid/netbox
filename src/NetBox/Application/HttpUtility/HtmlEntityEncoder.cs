using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace NetBox.Application.HttpUtility
{
   class HtmlEntityEncoder
   {
      private readonly IDictionary<char, HtmlEntity> _charToEntityMap;

      public HtmlEntityEncoder(IDictionary<char, HtmlEntity> charToEntityMap)
      {
         Debug.Assert(charToEntityMap != null);

         _charToEntityMap = charToEntityMap;
      }

      public void Encode(string html, TextWriter writer)
      {
         Debug.Assert(html != null);
         Debug.Assert(writer != null);

         foreach(char c in html)
         {
            HtmlEntity entity;
            if(_charToEntityMap.TryGetValue(c, out entity))
            {
               WriteEntity(c, entity, writer);
            }
            else
            {
               WriteChar(c, writer);
            }
         }
      }

      private static void WriteChar(char c, TextWriter writer)
      {
         writer.Write(c);
      }

      private static void WriteEntity(char c, HtmlEntity entity, TextWriter writer)
      {
         writer.Write(HtmlEncodingServices.EntityStartChar);

         if(entity.WriteAsDecimal)
         {
            WriteDecimalEntity(c, writer);
         }
         else
         {
            WriteTextEntity(entity, writer);
         }

         writer.Write(HtmlEncodingServices.EntityEndChar);
      }

      private static void WriteDecimalEntity(char c, TextWriter writer)
      {
         writer.Write(HtmlEncodingServices.NumericEntityStartChar);
         writer.Write(((int)c).ToString(CultureInfo.InvariantCulture));
      }

      private static void WriteTextEntity(HtmlEntity entity, TextWriter writer)
      {
         writer.Write(entity.Entity);
      }
   }
}