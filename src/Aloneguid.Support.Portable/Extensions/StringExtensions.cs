#if PORTABLE
extern alias PclContribRuntime;
#endif
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Aloneguid.Support;
using Aloneguid.Support.Application;
using Aloneguid.Support.Model;

// ReSharper disable once CheckNamespace
namespace System
{
   public static class StringExtensions
   {
      private const string HtmlStripPattern = @"<(.|\n)*?>";
      private static readonly JsonSerialiser Json = new JsonSerialiser();

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

      public static T XmlDeserialise<T>(this string s) where T : class, new()
      {
         return new XmlSerialiser().Deserialise<T>(s, G.Enc);
      }

      public static object XmlDeserialise(this string s, Type t)
      {
         return new XmlSerialiser().Deserialise(t, s, G.Enc);
      }

      public static T AsJsonObject<T>(this string s) where T : class, new()
      {
         return Json.Deserialise<T>(s);
      }

      public static object AsJsonObject(this string s, Type t)
      {
         return Json.Deserialise(s, t);
      }

      public static string Base64Encode(this string s)
      {
         if(s == null) return null;

         return Convert.ToBase64String(G.Enc.GetBytes(s));
      }

      public static string Base64Decode(this string s)
      {
         if(s == null) return null;

         byte[] data = Convert.FromBase64String(s);

         return G.Enc.GetString(data, 0, data.Length);
      }

#if PORTABLE

      public static string HtmlEncode(this string s)
      {
         return PclContribRuntime::System.Net.WebUtility.HtmlEncode(s);
      }

      public static string HtmlDecode(this string s)
      {
         return PclContribRuntime::System.Net.WebUtility.HtmlDecode(s);
      }

#else

      public static string HtmlEncode(this string s)
      {
         return WebUtility.HtmlEncode(s);
      }

      public static string HtmlDecode(this string s)
      {
         return WebUtility.HtmlDecode(s);
      }

#endif

      public static string GetHash(this string s, Encoding encoding, HashType hashType)
      {
         if (s == null) return null;
         if (encoding == null) encoding = Encoding.UTF8;

         byte[] input = encoding.GetBytes(s);
         byte[] hash = input.GetHash(hashType);
         return hash.ToHexString();
      }

      public static string GetHash(this string s, HashType hashType)
      {
         return GetHash(s, Encoding.UTF8, hashType);
      }
   }
}
