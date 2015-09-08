using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Aloneguid.Support;
using Aloneguid.Support.Application;
using Aloneguid.Support.Model;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// String extensions.
   /// </summary>
   public static class StringExtensions
   {
      private const string HtmlStripPattern = @"<(.|\n)*?>";
      private static readonly JsonSerialiser Json = new JsonSerialiser();
      static readonly char[] Invalid = Path.GetInvalidFileNameChars();

      /// <summary>
      /// Strips HTML string from any tags leaving text only.
      /// </summary>
      /// <param name="s"></param>
      /// <returns></returns>
      public static string StripHtml(this string s)
      {
         if (s == null) return null;

         //This pattern Matches everything found inside html tags;
         //(.|\n) - > Look for any character or a new line
         // *?  -> 0 or more occurences, and make a non-greedy search meaning
         //That the match will stop at the first available '>' it sees, and not at the last one
         //(if it stopped at the last one we could have overlooked
         //nested HTML tags inside a bigger HTML tag..)

         return Regex.Replace(s, HtmlStripPattern, string.Empty);
      }

      /// <summary>
      /// Deserialises object represented as XML string to a real object.
      /// </summary>
      /// <typeparam name="T">Object type.</typeparam>
      /// <param name="s">XML representation.</param>
      /// <returns>Object instance.</returns>
      public static T XmlDeserialise<T>(this string s) where T : class, new()
      {
         return new XmlSerialiser().Deserialise<T>(s, G.Enc);
      }

      /// <summary>
      /// Deserialises object represented as XML string to a real object.
      /// </summary>
      /// <param name="s">XML representation.</param>
      /// <param name="t">Object type.</param>
      /// <returns>Object instance.</returns>
      public static object XmlDeserialise(this string s, Type t)
      {
         return new XmlSerialiser().Deserialise(t, s, G.Enc);
      }

      /// <summary>
      /// Deserialises object represented as JSON string to a real object
      /// </summary>
      /// <typeparam name="T">Object type</typeparam>
      /// <param name="s">JSON representation.</param>
      /// <returns>Object instance</returns>
      public static T AsJsonObject<T>(this string s) where T : class, new()
      {
         return Json.Deserialise<T>(s);
      }

      /// <summary>
      /// Deserialises object represented as JSON string to a real object
      /// </summary>
      /// <param name="s">JSON representation.</param>
      /// <param name="t">Object type.</param>
      /// <returns>Object instance</returns>
      public static object AsJsonObject(this string s, Type t)
      {
         return Json.Deserialise(s, t);
      }

      /// <summary>
      /// Encodes a string to BASE64 format
      /// </summary>
      public static string Base64Encode(this string s)
      {
         if(s == null) return null;

         return Convert.ToBase64String(G.Enc.GetBytes(s));
      }

      /// <summary>
      /// Decodes a BASE64 encoded string
      /// </summary>
      public static string Base64Decode(this string s)
      {
         if(s == null) return null;

         byte[] data = Convert.FromBase64String(s);

         return G.Enc.GetString(data, 0, data.Length);
      }

      private static string GetHash(this string s, Encoding encoding, HashType hashType)
      {
         if (s == null) return null;
         if (encoding == null) encoding = Encoding.UTF8;

         byte[] input = encoding.GetBytes(s);
         byte[] hash = input.GetHash(hashType);
         return hash.ToHexString();
      }

      /// <summary>
      /// Gets string hash
      /// </summary>
      /// <param name="s">Source string</param>
      /// <param name="hashType">Hash type</param>
      /// <returns></returns>
      public static string GetHash(this string s, HashType hashType)
      {
         return GetHash(s, Encoding.UTF8, hashType);
      }

      /// <summary>
      /// Converts to MemoryStream with a specific encoding
      /// </summary>
      public static MemoryStream ToMemoryStream(this string s, Encoding encoding)
      {
         if (s == null) return null;
         if (encoding == null) encoding = Encoding.UTF8;

         return new MemoryStream(encoding.GetBytes(s));
      }

      /// <summary>
      /// Converts to MemoryStream in UTF-8 encoding
      /// </summary>
      public static MemoryStream ToMemoryStream(this string s)
      {
         return ToMemoryStream(s, null);
      }

      public static string SanitizePath(this string path)
      {
         return SanitizePath(path, ' ');
      }

      public static string SanitizePath(this string path, char replacement)
      {
         if (Invalid.Contains(replacement)) throw new ArgumentException("replacement char " + replacement + " is not a valid path char");

         if (string.IsNullOrEmpty(path)) return path;
         int first = path.IndexOfAny(Invalid);
         if (first == -1) return path;

         int length = path.Length;
         var result = new StringBuilder(length);
         result.Append(path, 0, first);
         result.Append(replacement);

         // convert the rest of the chars one by one 
         for (int i = first + 1; i < length; i++)
         {
            char ch = path[i];
            if (Invalid.Contains(ch))
            {
               // invalid char => append replacement
               result.Append(replacement);
            }
            else
            {
               // valid char
               result.Append(ch);
            }
         }

         return result.ToString();

      }

      /// <summary>
      /// Filesystem style widlcard match where * stands for any characters of any length and ? standa for one character
      /// </summary>
      /// <param name="s">input string</param>
      /// <param name="wildcard">wildcard</param>
      /// <returns>True if matches, false otherwise</returns>
      public static bool MatchesWildcard(this string s, string wildcard)
      {
         if (s == null) return false;
         if (wildcard == null) return false;

         wildcard = wildcard
            .Replace(".", "\\.")   //escape '.' as it's a regex character
            .Replace("*", ".*")
            .Replace("?", ".");
         var rgx = new Regex(wildcard, RegexOptions.IgnoreCase | RegexOptions.Singleline);

         return rgx.IsMatch(s);
      }

   }
}
