//extern alias Portable;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Aloneguid.Support.Tests.Extensions
{
   
   public class StringExtensionsTest : TestBase
   {
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

      /*[Fact]
      public void PortableXmlDeserialise_Null_Null()
      {
         XmlDoc doc = Portable::System.StringExtensions.XmlDeserialise<XmlDoc>((string)null);
         Assert.Null(doc);
      }*/

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


      /*[Fact]
      public void PortableXmlSerialiseDeserialise_Object_ValidString()
      {
         XmlDoc d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = Portable::System.ObjectExtensions.XmlSerialise(d1);
         XmlDoc d2 = Portable::System.StringExtensions.XmlDeserialise<XmlDoc>(s);
         XmlDoc d3 = (XmlDoc)Portable::System.StringExtensions.XmlDeserialise(s, typeof(XmlDoc));

         Assert.Equal("test", d2.SV);
         Assert.Equal("test", d3.SV);
      }*/


      [Fact]
      public void XmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         Assert.Throws<InvalidOperationException>(() => ho.XmlSerialise());
      }

      /*[Fact]
      public void PortableXmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();

         Assert.Throws<InvalidOperationException>(() => Portable::System.ObjectExtensions.XmlSerialise(ho));
      }*/

      [Fact]
      public void XmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Assert.Throws<InvalidOperationException>(() => nxo.XmlSerialise());
      }

      /*[Fact]
      public void PortableXmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Assert.Throws<InvalidOperationException>(() => Portable::System.ObjectExtensions.XmlSerialise(nxo));
      }*/

      [Fact]
      public void JsonSerialise_Object_ValidString()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.ToJsonString();
         Assert.NotNull(s);

         XmlDoc d2 = s.AsJsonObject<XmlDoc>();

         Assert.Equal(d1.SV, d2.SV);
      }

      [Fact]
      public void JsonSerialise_Object_GetAsArrayReturnsNull()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.ToJsonString();

         string[] array = s.AsJsonObject<string[]>();
         Assert.Null(array);
      }

      [Fact]
      public void JsonSerialise_Array_Deserialises()
      {
         string[] array = { "1", "2" };

         string s = array.ToJsonString();
         string[] array2 = s.AsJsonObject<string[]>();

         Assert.Equal(2, array2.Length);
         Assert.Equal("1", array2[0]);
         Assert.Equal("2", array2[1]);
      }

      [Fact]
      public void JsonSerialise_ObjectAsCompressed_SmallerString()
      {
         var odoc = new XmlDoc { SV = "value" };

         string fullString = odoc.ToJsonString();
         string compString = odoc.ToCompressedJsonString();

         Assert.True(fullString.Length > compString.Length);
      }

      /*[Fact]
      public void PortableJsonSerialise_Object_ValidString()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = Portable::System.ObjectExtensions.ToJsonString(d1);
         Assert.NotNull(s);

         XmlDoc d2 = Portable::System.StringExtensions.AsJsonObject<XmlDoc>(s);

         Assert.Equal(d1.SV, d2.SV);
      }*/


      [Fact]
      public void Base64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = s.Base64Encode();
         string s2 = s64.Base64Decode();

         Assert.Equal(s, s2);
      }

      /*[Fact]
      public void PortableBase64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = Portable::System.StringExtensions.Base64Encode(s);
         string s2 = Portable::System.StringExtensions.Base64Decode(s64);

         Assert.Equal(s, s2);
      }*/

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

      /*[Fact]
      public void PortableToMemoryStream_Variable_Variable()
      {
         string input = "test stream";
         using (var ms = Portable::System.StringExtensions.ToMemoryStream(input))
         {
            string s = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal(input, s);
         }
      }*/

      [InlineData("the?path", ' ', "the path")]
      [InlineData(null, ' ', null)]
      [InlineData("lo?ts\\of-charac&ter.s", '_', "lo_ts_of-charac&ter.s")]
      public void SanitizePath_Variable_Variable(string input, char replacement, string sanitized)
      {
         Assert.Equal(input.SanitizePath(replacement), sanitized);
         //Assert.Equal(Portable::System.StringExtensions.SanitizePath(input, replacement), sanitized);
      }

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

      [Fact]
      public void Compress_Array_Decompresses()
      {
         string input = GetTestData("book.txt").ToString(Encoding.UTF8);
         byte[] output = input.Gzip(Encoding.UTF8);
         Assert.True(output.Length < input.Length);

         string unzipped = Encoding.UTF8.GetString(output.Ungzip());
         Assert.Equal(input, unzipped);
      }

      [Fact]
      public void ExtractTextBetween_ReturnsNullIfStartTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.Null(s.FindTagged("Sean", "test", false));
      }

      [Fact]
      public void ExtractTextBetween_ReturnsNullIfEndTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.Null(s.FindTagged("This", "Sean", false));
      }

      [Fact]
      public void ExtractTextBetween_DoesNotRemoveOuterTokens()
      {
         string s = "This is a test";
         Assert.Equal(" is a ", s.FindTagged(" is", "a ", true));
      }

      [Fact]
      public void ExtractTextBetween_RemovesOuterTokens()
      {
         string s = "This is a test";
         Assert.Equal(" ", s.FindTagged(" is", "a ", false));
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

      [InlineData("One Two", "OneTwo")]
      [InlineData("one two Three", "OneTwoThree")]
      [InlineData("one tWo Three", "OneTwoThree")]
      [InlineData(null, null)]
      [InlineData("one tw", "OneTw")]
      public void SpacedToCamelCase_Variable_Variable(string input, string expected)
      {
         Assert.Equal(expected, input.SpacedToCamelCase());
      }

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

      // ReSharper disable once MemberCanBePrivate.Global
      public class XmlDoc
      {
         // ReSharper disable once InconsistentNaming
         public string SV { get; set; }
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
