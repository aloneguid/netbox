using System;

namespace Aloneguid.Support.Application
{
   static class StringManipulation
   {
      private static bool FindTextBetween(ref string s, ref string startToken, ref string endToken, bool includeOuterTokens, out int startIdx, out int length)
      {
         int idx0 = s.IndexOf(startToken, StringComparison.Ordinal);

         if(idx0 != -1)
         {
            int idx1 = s.IndexOf(endToken, idx0, StringComparison.Ordinal);

            if(idx1 != -1)
            {
               startIdx = includeOuterTokens ? idx0 : idx0 + startToken.Length;
               length = includeOuterTokens
                            ? (idx1 - idx0 + endToken.Length)
                            : (idx1 - idx0 - startToken.Length);

               return true;
            }
         }

         startIdx = length = -1;
         return false;
      }

      public static string ExtractTextBetween(string s, string startToken, string endToken, bool includeOuterTokens)
      {
         if(startToken == null) throw new ArgumentNullException(nameof(startToken));
         if(endToken == null) throw new ArgumentNullException(nameof(endToken));

         int start, length;
         if(FindTextBetween(ref s, ref startToken, ref endToken, includeOuterTokens, out start, out length))
         {
            return s.Substring(start, length);
         }

         return null;
      }

      public static string ReplaceTextBetween(string s, string startToken, string endToken, string replacementText, bool replaceOuterTokens)
      {
         if(startToken == null) throw new ArgumentNullException(nameof(startToken));
         if(endToken == null) throw new ArgumentNullException(nameof(endToken));
         //if (replacementText == null) throw new ArgumentNullException("replacementText");

         int start, length;

         if(FindTextBetween(ref s, ref startToken, ref endToken, replaceOuterTokens, out start, out length))
         {
            s = s.Remove(start, length);

            if(replacementText != null)
               s = s.Insert(start, replacementText);
         }

         return s;
      }
   }
}
