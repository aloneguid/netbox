namespace NetBox
{
   /// <summary>
   /// Common JavaScript helpers
   /// </summary>
   public static class JavaScriptUtils
   {
      /// <summary>
      /// Escapes a string for JSON encoding
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string ToEscapedJavaScriptString(string s)
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
   }
}
