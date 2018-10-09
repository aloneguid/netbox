using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetBox.Extensions;
using NetBox.Generator;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class StringExtensionsTest : TestBase
   {
      [Theory]
      [InlineData("<string>test text</string>", "test text")]
      public void StripHtml_Variable_Variable(string html, string stripped)
      {
         Assert.Equal(stripped, html.StripHtml());
      }

      [Fact]
      public void XmlDeserialise_Null_Null()
      {
         XmlDoc doc = ((string)null).XmlDeserialise<XmlDoc>();
         Assert.Null(doc);
      }

      [Fact]
      public void XmlSerialiseDeserialise_Object_ValidString()
      {
         XmlDoc d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.XmlSerialise();
         XmlDoc d2 = s.XmlDeserialise<XmlDoc>();
         XmlDoc d3 = (XmlDoc)s.XmlDeserialise(typeof(XmlDoc));

         Assert.Equal("test", d2.SV);
         Assert.Equal("test", d3.SV);
      }

      [Fact]
      public void JsonSerialiseDeserialise_Object_ValidString()
      {
         XmlDoc d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.JsonSerialise();
         XmlDoc d2 = (XmlDoc)s.JsonDeserialise(typeof(XmlDoc));
         XmlDoc d3 = s.JsonDeserialise<XmlDoc>();

         Assert.Equal("test", d2.SV);
         Assert.Equal("test", d3.SV);
      }

      [Fact]
      public void XmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         Assert.Throws<InvalidOperationException>(() => ho.XmlSerialise());
      }

      [Fact]
      public void XmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Assert.Throws<InvalidOperationException>(() => nxo.XmlSerialise());
      }

      [Fact]
      public void Base64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = s.Base64Encode();
         string s2 = s64.Base64Decode();

         Assert.Equal(s, s2);
      }

      [Fact]
      public void ToMemoryStream_TestString_ReadsBack()
      {
         string input = "test stream";
         using(var ms = input.ToMemoryStream())
         {
            string s = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal(input, s);
         }
      }

      [Fact]
      public void ToMemoryStream_EncodingTestString_ReadsBack()
      {
         string input = "test stream";
         using (var ms = input.ToMemoryStream(Encoding.ASCII))
         {
            string s = Encoding.ASCII.GetString(ms.ToArray());
            Assert.Equal(input, s);
         }
      }

      [Theory]
      [InlineData("the?path", ' ', "the path")]
      [InlineData(null, ' ', null)]
      [InlineData("lo?ts\\of-charac&ter.s", '_', "lo_ts_of-charac&ter.s")]
      public void SanitizePath_Variable_Variable(string input, char replacement, string sanitized)
      {
         Assert.Equal(input.SanitizePath(replacement), sanitized);
         //Assert.Equal(Portable::System.StringExtensions.SanitizePath(input, replacement), sanitized);
      }

      [Theory]
      [InlineData("file.jpg", "*.jpg", true)]
      [InlineData("file.jpeg", "*.jpg", false)]
      [InlineData("x264-Human-720p.mkv", "*Human*", true)]
      [InlineData("x264-Human-720p.mkv", "*Human*.mkv", true)]
      [InlineData("x264-Human-720p.mkv", "*Human*.avi", false)]
      [InlineData(null, "*", false)]
      [InlineData("file.jpg", null, false)]
      public void MatchesWildcard_Variable_Variable(string input, string wildcard, bool isMatch)
      {
         Assert.Equal(isMatch, input.MatchesWildcard(wildcard));
         //Assert.Equal(isMatch, Portable::System.StringExtensions.MatchesWildcard(input, wildcard));
      }

      [Theory]
      [InlineData("<strong>entity</strong>", "&lt;strong&gt;entity&lt;/strong&gt;")]
      [InlineData(null, null)]
      public void HtmlEncodeDecode_Variable_Variable(string decoded, string encoded)
      {
         string encodedFull = decoded.HtmlEncode();
         //string encodedPort = Portable::System.StringExtensions.HtmlEncode(decoded);

         //Assert.Equal(encodedFull, encodedPort);
         Assert.Equal(encoded, encodedFull);

         string decodedFull = encoded.HtmlDecode();
         //string decodedPort = Portable::System.StringExtensions.HtmlDecode(encoded);

         //Assert.Equal(decodedFull, decodedPort);
         Assert.Equal(decoded, decodedFull);
      }

      [Theory]
      [InlineData(null, null)]
      [InlineData("the string", "the+string")]
      [InlineData("lk>i*", "lk%3Ei*")]
      public void UrlEncode_Variable_Variable(string decoded, string encoded)
      {
         string encodedNow = decoded.UrlEncode();

         Assert.Equal(encoded, encodedNow);
      }

      [Theory]
      [InlineData("the %variable%", "%", "%", true, "%variable%")]
      [InlineData("the %variable%", "%", "%", false, "variable")]
      [InlineData("this is a test", "Sean", "test", false, null)]
      [InlineData("this is a test", " is", "test", false, " a ")]
      public void FindTagged_Variations(string input, string startTag, string endTag, bool includeOuter, string expected)
      {
         Assert.Equal(expected, input.FindTagged(startTag, endTag, includeOuter));
      }

      [Fact]
      public void ReplaceTextBetween_ReturnsPastInStringIfStartTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.Equal("this is a test", s.ReplaceTagged("Sean", "test", "me", false));
      }

      [Fact]
      public void ReplaceTextBetween_ReturnsPastInStringIfEndTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.Equal("this is a test", s.ReplaceTagged("This", "Sean", "me", false));
      }

      [Fact]
      public void ReplaceTextBetween_RemovesOuterTokens()
      {
         string s = "This is a test";
         Assert.Equal("This unit test", s.ReplaceTagged(" is", "a ", " unit ", true));
      }

      [Fact]
      public void ReplaceTextBetween_DoesNotRemoveOuterTokens()
      {
         string s = "This is a test";
         Assert.Equal("This is unit a test", s.ReplaceTagged(" is", "a ", " unit ", false));
      }

      [Theory]
      [InlineData("One Two", "OneTwo")]
      [InlineData("one two Three", "OneTwoThree")]
      [InlineData("one tWo Three", "OneTwoThree")]
      [InlineData(null, null)]
      [InlineData("one tw", "OneTw")]
      public void SpacedToCamelCase_Variable_Variable(string input, string expected)
      {
         Assert.Equal(expected, input.SpacedToCamelCase());
      }

      [Theory]
      [InlineData(null, null)]
      [InlineData("O", "O")]
      [InlineData("o", "O")]
      [InlineData("one", "One")]
      [InlineData("tWo", "Two")]
      [InlineData("1234", "1234")]
      public void Capitalize_Variable_Variable(string input, string expected)
      {
         Assert.Equal(expected, input.Capitalize());
      }

      [Theory]
      [InlineData(null, null, null, null)]
      [InlineData("word", "ord", 1, null)]
      [InlineData("word", "ord", -3, null)]
      [InlineData("word", "word", -4, null)]
      [InlineData("word", "rd", -2, null)]
      [InlineData("word", "word", 0, null)]
      [InlineData("word", "word", null, null)]
      [InlineData("word", "wor", null, 3)]
      [InlineData("word", "wo", null, -2)]
      [InlineData("word", "", null, -10)]
      [InlineData("word", "or", 1, -1)]
      public void Slice_Variable_Variable(string input, string expected, int? start, int? end)
      {
         string result = input.Slice(start, end);

         Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData(null, null, null, null)]
      [InlineData("key:value", null, "key:value", null)]
      [InlineData("key:value", ":", "key", "value")]
      [InlineData("key:value", "=", "key:value", null)]
      [InlineData("key:", ":", "key", "")]
      public void SplitByDelimiter_Variable_Variable(string input, string delimiter, string expectedKey, string expectedValue)
      {
         Tuple<string, string> result = input.SplitByDelimiter(delimiter);

         string key = result?.Item1;
         string value = result?.Item2;

         Assert.Equal(expectedKey, key);
         Assert.Equal(expectedValue, value);
      }

      [Fact]
      public void Empty_string_from_hex_returns_empty_byte_array()
      {
         byte[] r = "".FromHexToBytes();
         Assert.NotNull(r);
         Assert.Empty(r);
      }

      [Fact]
      public void Null_string_from_hex_returns_null()
      {
         byte[] r = ((string)null).FromHexToBytes();
         Assert.Null(r);
      }

      [Fact]
      public void Invalid_hex_from_hex_returns_null()
      {
         byte[] r = "zztop".FromHexToBytes();
         Assert.Null(r);
      }

      [Fact]
      public void One_byte_from_hex_returns_empty_byte_array()
      {
         byte[] r = "z".FromHexToBytes();
         Assert.Empty(r);

      }

      [Fact]
      public void DataProtection_Protect_Unprotect_in_user_scope()
      {
         string input = RandomGenerator.RandomString;
         string encrypted = input.Protect(false);

         Assert.Equal(input, encrypted.Unprotect(false));
      }

      [Fact]
      public void DataProtection_Protect_Unprotect_in_machine_scope()
      {
         string input = RandomGenerator.RandomString;
         string encrypted = input.Protect(true);

         Assert.Equal(input, encrypted.Unprotect(true));
      }

      [Fact]
      public void DataProtection_Unprotect_Nonbase64()
      {
         Assert.Throws<ArgumentException>(() => "123".Unprotect());
      }

      [Fact]
      public void DataProtection_Unprotect_InvalidData()
      {
         string incorrect = "123".Protect(false);

         Assert.Equal("123", incorrect.Unprotect(true));
      }


      /*[Fact]
      public async Task I_can_download_web_page()
      {
         string content = await ("http://microsoft.com".HttpGetAsync());

         Assert.NotNull(content);
      }*/

      // ReSharper disable once MemberCanBePrivate.Global
      public class XmlDoc
      {
         // ReSharper disable once InconsistentNaming
         public string SV { get; set; }

         //public XmlEnum E { get; set; }
      }

      public enum XmlEnum
      {
         One,
         Two
      }

      private class HiddenDoc
      {
         // ReSharper disable once InconsistentNaming
         public string SV { get; set; }
      }

      // ReSharper disable once MemberCanBePrivate.Global
      public class NonXmlDoc
      {
         public NonXmlDoc(int i)
         {

         }
      }

   }
}
