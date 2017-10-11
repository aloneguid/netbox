using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NetBox;
using NetBox.Application;
using NetBox.Model;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using WebUtility = NetBox.Application.WebUtility;
using System.Threading.Tasks;
#if NETSTANDARD
using NetBox.Application.HttpUtility;
using System.Globalization;
#else
using System.Web;
#endif

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
      /// Convers hex string to byte array
      /// </summary>
      /// <param name="hex"></param>
      /// <returns></returns>
      public static byte[] FromHexToBytes(this string hex)
      {
         if (hex == null) return null;

         byte[] raw = new byte[hex.Length / 2];
         for (int i = 0; i < raw.Length; i++)
         {
            try
            {
               raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            catch(FormatException)
            {
               return null;
            }
         }
         return raw;
      }

      #region [ Web Helpers ]

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

#if NETSTANDARD
      /// <summary>
      /// Encodes to HTML string
      /// </summary>
      public static string HtmlEncode(this string value)
      {
         if (string.IsNullOrEmpty(value)) return value;

         using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
         {
            HtmlEncode(value, writer);

            return writer.ToString();
         }
      }

      private static void HtmlEncode(string value, TextWriter output)
      {
         // Very weird behavior that we don't throw on a null value, but 
         // do on empty, however, this mimics the platform implementation
         if (value == null) return;
         if (output == null) throw new ArgumentNullException(nameof(output));

         HtmlEncodingServices.Encode(value, output);
      }
#else
      /// <summary>
      /// Encodes to HTML string
      /// </summary>
      public static string HtmlEncode(this string value)
      {
         return HttpUtility.HtmlEncode(value);
      }
#endif

#if NETSTANDARD
      /// <summary>
      /// Decodes from HTML string
      /// </summary>
      public static string HtmlDecode(this string value)
      {
         if (string.IsNullOrEmpty(value)) return value;

         using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
         {
            HtmlDecode(value, writer);

            return writer.ToString();
         }
      }

      private static void HtmlDecode(string value, TextWriter output)
      {
         // Very weird behavior that we don't throw on a null value, but 
         // do on empty, however, this mimics the platform implementation
         if (value == null) return;
         if (output == null) throw new ArgumentNullException(nameof(output));

         HtmlEncodingServices.Decode(value, output);
      }
#else
      /// <summary>
      /// Decodes from HTML string
      /// </summary>
      public static string HtmlDecode(this string value)
      {
         return HttpUtility.HtmlDecode(value);
      }
#endif

      /// <summary>
      /// URL-encodes input string
      /// </summary>
      /// <param name="value">String to encode</param>
      /// <returns>Encoded string</returns>
      public static string UrlEncode(this string value)
      {
         return WebUtility.UrlEncode(value);
      }

      /// <summary>
      /// URL-decodes input string
      /// </summary>
      /// <param name="value">String to decode</param>
      /// <returns>Decoded string</returns>
      public static string UrlDecode(this string value)
      {
         return WebUtility.UrlDecode(value);
      }

      #endregion

      #region [ Serialization ]

#if (NETFULL || NETSTANDARD20)

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
#endif

      /// <summary>
      /// Deserialises object represented as JSON string to a real object
      /// </summary>
      /// <typeparam name="T">Object type</typeparam>
      /// <param name="s">JSON representation.</param>
      /// <returns>Object instance</returns>
      public static T AsJsonObject<T>(this string s)
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

      #endregion

      #region [ Encoding ]

      /// <summary>
      /// Encodes a string to BASE64 format
      /// </summary>
      public static string Base64Encode(this string s)
      {
         if (s == null) return null;

         return Convert.ToBase64String(G.Enc.GetBytes(s));
      }

      /// <summary>
      /// Decodes a BASE64 encoded string
      /// </summary>
      public static string Base64Decode(this string s)
      {
         if (s == null) return null;

         byte[] data = Convert.FromBase64String(s);

         return G.Enc.GetString(data, 0, data.Length);
      }

      /// <summary>
      /// Decodes a BASE64 encoded string to byte array
      /// </summary>
      /// <param name="s">String to decode</param>
      /// <returns>Byte array</returns>
      public static byte[] Base64DecodeAsBytes(this string s)
      {
         if (s == null) return null;

         return Convert.FromBase64String(s);
      }

      /// <summary>
      /// Converts shortest guid representation back to Guid. See <see cref="GuidExtensions.ToShortest(Guid)"/>
      /// on how to convert Guid to string.
      /// </summary>
      public static Guid FromShortestGuid(this string s)
      {
         byte[] guidBytes = Ascii85.Instance.Decode(s, false);
         return new Guid(guidBytes);
      }

      #endregion

      #region [ Hashing ]

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

      #endregion

      #region [ Stream Conversion ]

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
         // ReSharper disable once IntroduceOptionalParameters.Global
         return ToMemoryStream(s, null);
      }

      #endregion

      #region [ Filesystem ]
      /// <summary>
      /// Removes invalid path characters from the string, replacing them by space (' ') character
      /// </summary>
      public static string SanitizePath(this string path)
      {
         // ReSharper disable once IntroduceOptionalParameters.Global
         return SanitizePath(path, ' ');
      }

      /// <summary>
      /// Removes invalid path characters from the string, replacing them by the given character
      /// </summary>
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

      #endregion

      #region [ GZip ]

      /// <summary>
      /// Gzips a specified string into array of bytes using specified encoding
      /// </summary>
      public static byte[] Gzip(this string s, Encoding encoding)
      {
         if (s == null) return null;
         if (encoding == null) throw new ArgumentNullException(nameof(encoding));

         byte[] data = encoding.GetBytes(s);
         return data.Gzip();
      }

      /// <summary>
      /// Gzips a specified string in specified encoding to to destination stream.
      /// </summary>
      public static void Gzip(this string s, Encoding encoding, Stream destinationStream)
      {
         if (s == null) return;
         if (encoding == null) throw new ArgumentNullException(nameof(encoding));
         if (destinationStream == null) throw new ArgumentNullException(nameof(destinationStream));

         using (var ms = new MemoryStream(encoding.GetBytes(s)))
         {
            Compressor.Compress(ms, destinationStream);
         }
      }

      #endregion

      #region [ String Manipulation ]

      /// <summary>
      /// Looks for <paramref name="startTag"/> and <paramref name="endTag"/> followed in sequence and when found returns the text between them.
      /// </summary>
      /// <param name="s">Input string</param>
      /// <param name="startTag">Start tag</param>
      /// <param name="endTag">End tag</param>
      /// <param name="includeOuterTags">When set to true, returns the complete phrase including start and end tag value,
      /// otherwise only inner text returned</param>
      /// <returns></returns>
      public static string FindTagged(this string s, string startTag, string endTag, bool includeOuterTags)
      {
         return StringManipulation.ExtractTextBetween(s, startTag, endTag, includeOuterTags);
      }

      /// <summary>
      /// Looks for <paramref name="startTag"/> and <paramref name="endTag"/> followed in sequence, and if found
      /// performs a replacement of text inside them with <paramref name="replacementText"/>
      /// </summary>
      /// <param name="s">Input string</param>
      /// <param name="startTag">Start tag</param>
      /// <param name="endTag">End tag</param>
      /// <param name="replacementText">Replacement text</param>
      /// <param name="replaceOuterTokens">When set to true, not only the text between tags is replaced, but the whole
      /// phrase with <paramref name="startTag"/>, text between tags and <paramref name="endTag"/></param>
      /// <returns></returns>
      public static string ReplaceTagged(this string s, string startTag, string endTag, string replacementText, bool replaceOuterTokens)
      {
         return StringManipulation.ReplaceTextBetween(s, startTag, endTag, replacementText, replaceOuterTokens);
      }

      /// <summary>
      /// Converts a string with spaces to a camel case version, for example
      /// "The camel string" is converted to "TheCamelString"
      /// </summary>
      public static string SpacedToCamelCase(this string s)
      {
         return StringManipulation.SpacedToCamelCase(s);
      }

      /// <summary>
      /// Transforms the string so that the first letter is uppercase and the rest of them are lowercase
      /// </summary>
      public static string Capitalize(this string s)
      {
         return StringManipulation.Capitalise(s);
      }

      /// <summary>
      /// Pythonic approach to slicing strings
      /// </summary>
      /// <param name="s">Input string</param>
      /// <param name="start">Is the start index to slice from. It can be either positive or negative.
      /// Negative value indicates that the index is taken from the end of the string.</param>
      /// <param name="end">Is the index to slice to. It can be either positive or negative.
      /// Negative value indicates that the index is taken from the end of the string.</param>
      /// <returns>Sliced string</returns>
      public static string Slice(this string s, int? start, int? end)
      {
         if (s == null) return null;
         if (start == null && end == null) return s;

         int si = start ?? 0;
         int ei = end ?? s.Length;

         if(si < 0)
         {
            si = s.Length + si;
            if (si < 0) si = 0;
         }

         if(si > s.Length)
         {
            si = s.Length - 1;
         }

         if(ei < 0)
         {
            ei = s.Length + ei;
            if (ei < 0) ei = 0;
         }

         if(ei > s.Length)
         {
            ei = s.Length;
         }

         return s.Substring(si, ei - si);
      }

      /// <summary>
      /// Splits the string into key and value using the provided delimiter values. Both key and value are trimmed as well.
      /// </summary>
      /// <param name="s">Input string. When null returns null immediately.</param>
      /// <param name="delimiter">List of delmiters between key and value. This method searches for all the provided
      /// delimiters, and splits by the first left-most one.</param>
      /// <returns>A tuple of two values where the first one is the key and second is the value. If none of the delimiters
      /// are found the second value of the tuple is null and the first value is the input string</returns>
      public static Tuple<string, string> SplitByDelimiter(this string s, params string[] delimiter)
      {
         if (s == null) return null;

         string key, value;

         if (delimiter == null || delimiter.Length == 0)
         {
            key = s.Trim();
            value = null;
         }
         else
         {

            List<int> indexes = delimiter.Where(d => d != null).Select(d => s.IndexOf(d)).Where(d => d != -1).ToList();

            if (indexes.Count == 0)
            {
               key = s.Trim();
               value = null;
            }
            else
            {
               int idx = indexes.OrderBy(i => i).First();
               key = s.Substring(0, idx);
               value = s.Substring(idx + 1);
            }
         }

         return new Tuple<string, string>(key, value);
      }

      #endregion

      #region [ Network Credential ]

      /// <summary>
      /// Converts string to network credentials if it's formatted in a friendly way.
      /// See <see cref="NetworkCredentialExtensions.ToFriendlyString(NetworkCredential)"/> how to do it.
      /// </summary>
      /// <param name="s">String to conver from.</param>
      /// <returns>An instance of <see cref="NetworkCredential"/></returns>
      public static NetworkCredential ToNetworkCredential(this string s)
      {
         if (s == null) return null;

         var credsAndDomain = s.SplitByDelimiter("@");
         string creds = credsAndDomain.Item1;
         string domain = credsAndDomain.Item2;

         var usernameAndPassword = creds.SplitByDelimiter(":");
         string username = usernameAndPassword.Item1;
         string password = usernameAndPassword.Item2;

         return new NetworkCredential(username, password, domain);
      }

      #endregion

      #region [ Daily Life ]

      /// <summary>
      /// Creates a key-value pair from this string and an object value
      /// </summary>
      public static KeyValuePair<string, T> PairedWith<T>(this string key, T value)
      {
         return new KeyValuePair<string, T>(key, value);
      }

      public static KeyValuePair<string, object> WithObj(this string key, object value)
      {
         return new KeyValuePair<string, object>(key, value);
      }

      #endregion

      /*
      #region [ HTTP ]

      public static async Task<string> HttpGetAsync(this string url, string contentType = null)
      {
         return await EasyHttp.HttpGetAsync(url, contentType);
      }

      #endregion
      */

   }
}