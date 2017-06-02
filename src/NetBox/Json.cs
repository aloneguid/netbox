using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetBox
{
   /// <summary>
   /// Common JavaScript helpers
   /// </summary>
   public static class Json
   {
      /// <summary>
      /// Escapes a string for JSON encoding
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string ToEscapedString(string s)
      {
         if (s == null) return null;

         string result = string.Empty;
         for (int i = 0; i < s.Length; i++)
         {
            char c = s[i];
            string ec;

            switch (c)
            {
               case '\t':
                  ec = "\\t";
                  break;
               case '\n':
                  ec = "\\n";
                  break;
               case '\r':
                  ec = "\\r";
                  break;
               case '\f':
                  ec = "\\f";
                  break;
               case '\b':
                  ec = "\\b";
                  break;
               case '\\':
                  ec = "\\\\";
                  break;
               case '\u0085': // Next Line
                  ec = "\\u0085";
                  break;
               case '\u2028': // Line Separator
                  ec = "\\u2028";
                  break;
               case '\u2029': // Paragraph Separator
                  ec = "\\u2029";
                  break;
               default:
                  ec = new string(c, 1);
                  break;
            }

            result += ec;
         }

         return result;
      }

      /// <summary>
      /// Flattens JSON document i.e. transorms to a key-value object.
      /// </summary>
      /// <param name="jsonDocument"></param>
      /// <param name="compress">When true, generates a compressed JSON document (one line, no formatting)</param>
      /// <example>
      /// The following document@
      /// 
      /// {
      ///   "p1": "v1",
      ///   "obj":
      ///   {
      ///      "p2" : "v2",
      ///      "p3" : "v3",
      ///      "obj1" :
      ///      {
      ///         "p4": 4
      ///      }
      ///   }
      /// }
      /// 
      /// is flattened into
      /// 
      /// {
      ///   "p1": "v1",
      ///   "obj.p2": "v2",
      ///   "obj.p3": "v3",
      ///   "obj.obj1.p4": 4
      /// }
      /// </example>
      /// <returns></returns>
      public static string Flatten(string jsonDocument, bool compress = false)
      {
         if (jsonDocument == null) return null;

         JObject j = JObject.Parse(jsonDocument);

         IEnumerable<JToken> leaves = j
            .Descendants()
            .Where(l => l.Children().Count() == 0);

         var map = new Dictionary<string, JToken>();
         foreach (JToken leaf in leaves)
         {
            map[leaf.Path] = leaf;
         }

         return map.ToJsonString(compress);
      }
   }
}
