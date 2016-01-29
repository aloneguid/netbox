extern alias Portable;
using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class StringExtensionsTest : TestBase
   {
      [TestCase("<string>test text</string>", "test text")]
      public void StripHtml_Variable_Variable(string html, string stripped)
      {
         Assert.AreEqual(stripped, html.StripHtml());
      }

      [Test]
      public void XmlDeserialise_Null_Null()
      {
         XmlDoc doc = ((string)null).XmlDeserialise<XmlDoc>();
         Assert.IsNull(doc);
      }

      [Test]
      public void PortableXmlDeserialise_Null_Null()
      {
         XmlDoc doc = Portable::System.StringExtensions.XmlDeserialise<XmlDoc>((string)null);
         Assert.IsNull(doc);
      }

      [Test]
      public void XmlSerialiseDeserialise_Object_ValidString()
      {
         XmlDoc d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.XmlSerialise();
         XmlDoc d2 = s.XmlDeserialise<XmlDoc>();
         XmlDoc d3 = (XmlDoc)s.XmlDeserialise(typeof(XmlDoc));

         Assert.AreEqual("test", d2.SV);
         Assert.AreEqual("test", d3.SV);
      }


      [Test]
      public void PortableXmlSerialiseDeserialise_Object_ValidString()
      {
         XmlDoc d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = Portable::System.ObjectExtensions.XmlSerialise(d1);
         XmlDoc d2 = Portable::System.StringExtensions.XmlDeserialise<XmlDoc>(s);
         XmlDoc d3 = (XmlDoc)Portable::System.StringExtensions.XmlDeserialise(s, typeof(XmlDoc));

         Assert.AreEqual("test", d2.SV);
         Assert.AreEqual("test", d3.SV);
      }


      [Test]
      public void XmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         Assert.Throws<InvalidOperationException>(() => ho.XmlSerialise());
      }

      [Test]
      public void PortableXmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();

         Assert.Throws<InvalidOperationException>(() => Portable::System.ObjectExtensions.XmlSerialise(ho));
      }

      [Test]
      public void XmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Assert.Throws<InvalidOperationException>(() => nxo.XmlSerialise());
      }

      [Test]
      public void PortableXmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Assert.Throws<InvalidOperationException>(() => Portable::System.ObjectExtensions.XmlSerialise(nxo));
      }

      [Test]
      public void JsonSerialise_Object_ValidString()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.ToJsonString();
         Assert.IsNotNull(s);

         XmlDoc d2 = s.AsJsonObject<XmlDoc>();

         Assert.AreEqual(d1.SV, d2.SV);
      }

      [Test]
      public void JsonSerialise_Object_GetAsArrayReturnsNull()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = d1.ToJsonString();

         string[] array = s.AsJsonObject<string[]>();
         Assert.IsNull(array);
      }

      [Test]
      public void JsonSerialise_Array_Deserialises()
      {
         string[] array = { "1", "2" };

         string s = array.ToJsonString();
         string[] array2 = s.AsJsonObject<string[]>();

         Assert.AreEqual(2, array2.Length);
         Assert.AreEqual("1", array2[0]);
         Assert.AreEqual("2", array2[1]);
      }

      [Test]
      public void JsonSerialise_ObjectAsCompressed_SmallerString()
      {
         var odoc = new XmlDoc { SV = "value" };

         string fullString = odoc.ToJsonString();
         string compString = odoc.ToCompressedJsonString();

         Assert.Greater(fullString.Length, compString.Length);
      }

      [Test]
      public void PortableJsonSerialise_Object_ValidString()
      {
         var d1 = new XmlDoc
         {
            SV = "test"
         };

         string s = Portable::System.ObjectExtensions.ToJsonString(d1);
         Assert.IsNotNull(s);

         XmlDoc d2 = Portable::System.StringExtensions.AsJsonObject<XmlDoc>(s);

         Assert.AreEqual(d1.SV, d2.SV);
      }


      [Test]
      public void Base64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = s.Base64Encode();
         string s2 = s64.Base64Decode();

         Assert.AreEqual(s, s2);
      }

      [Test]
      public void PortableBase64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = Portable::System.StringExtensions.Base64Encode(s);
         string s2 = Portable::System.StringExtensions.Base64Decode(s64);

         Assert.AreEqual(s, s2);
      }

      [Test]
      public void ToMemoryStream_TestString_ReadsBack()
      {
         string input = "test stream";
         using(var ms = input.ToMemoryStream())
         {
            string s = Encoding.UTF8.GetString(ms.ToArray());
            Assert.AreEqual(input, s);
         }
      }

      [Test]
      public void ToMemoryStream_EncodingTestString_ReadsBack()
      {
         string input = "test stream";
         using (var ms = input.ToMemoryStream(Encoding.ASCII))
         {
            string s = Encoding.ASCII.GetString(ms.ToArray());
            Assert.AreEqual(input, s);
         }
      }

      [Test]
      public void PortableToMemoryStream_Variable_Variable()
      {
         string input = "test stream";
         using (var ms = Portable::System.StringExtensions.ToMemoryStream(input))
         {
            string s = Encoding.UTF8.GetString(ms.ToArray());
            Assert.AreEqual(input, s);
         }
      }

      [TestCase("the?path", ' ', "the path")]
      [TestCase(null, ' ', null)]
      [TestCase("lo?ts\\of-charac&ter.s", '_', "lo_ts_of-charac&ter.s")]
      public void SanitizePath_Variable_Variable(string input, char replacement, string sanitized)
      {
         Assert.AreEqual(input.SanitizePath(replacement), sanitized);
         Assert.AreEqual(Portable::System.StringExtensions.SanitizePath(input, replacement), sanitized);
      }

      [TestCase("file.jpg", "*.jpg", true)]
      [TestCase("file.jpeg", "*.jpg", false)]
      [TestCase("x264-Human-720p.mkv", "*Human*", true)]
      [TestCase("x264-Human-720p.mkv", "*Human*.mkv", true)]
      [TestCase("x264-Human-720p.mkv", "*Human*.avi", false)]
      [TestCase(null, "*", false)]
      [TestCase("file.jpg", null, false)]
      public void MatchesWildcard_Variable_Variable(string input, string wildcard, bool isMatch)
      {
         Assert.AreEqual(isMatch, input.MatchesWildcard(wildcard));
         Assert.AreEqual(isMatch, Portable::System.StringExtensions.MatchesWildcard(input, wildcard));
      }

      [TestCase("<strong>entity</strong>", "&lt;strong&gt;entity&lt;/strong&gt;")]
      [TestCase(null, null)]
      public void HtmlEncodeDecode_Variable_Variable(string decoded, string encoded)
      {
         string encodedFull = decoded.HtmlEncode();
         string encodedPort = Portable::System.StringExtensions.HtmlEncode(decoded);

         Assert.AreEqual(encodedFull, encodedPort);
         Assert.AreEqual(encoded, encodedFull);

         string decodedFull = encoded.HtmlDecode();
         string decodedPort = Portable::System.StringExtensions.HtmlDecode(encoded);

         Assert.AreEqual(decodedFull, decodedPort);
         Assert.AreEqual(decoded, decodedFull);
      }

      [Test]
      public void Compress_Array_Decompresses()
      {
         string input = GetTestData("book.txt").ToString(Encoding.UTF8);
         byte[] output = input.Gzip(Encoding.UTF8);
         Assert.Less(output.Length, input.Length);

         string unzipped = Encoding.UTF8.GetString(output.Ungzip());
         Assert.AreEqual(input, unzipped);
      }

      [Test]
      public void ExtractTextBetween_ReturnsNullIfStartTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.IsNull(s.FindTagged("Sean", "test", false));
      }

      [Test]
      public void ExtractTextBetween_ReturnsNullIfEndTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.IsNull(s.FindTagged("This", "Sean", false));
      }

      [Test]
      public void ExtractTextBetween_DoesNotRemoveOuterTokens()
      {
         string s = "This is a test";
         Assert.AreEqual(" is a ", s.FindTagged(" is", "a ", true));
      }

      [Test]
      public void ExtractTextBetween_RemovesOuterTokens()
      {
         string s = "This is a test";
         Assert.AreEqual(" ", s.FindTagged(" is", "a ", false));
      }

      [Test]
      public void ReplaceTextBetween_ReturnsPastInStringIfStartTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.AreEqual("this is a test", s.ReplaceTagged("Sean", "test", "me", false));
      }

      [Test]
      public void ReplaceTextBetween_ReturnsPastInStringIfEndTokenDoesNotExistInPassedInString()
      {
         string s = "this is a test";
         Assert.AreEqual("this is a test", s.ReplaceTagged("This", "Sean", "me", false));
      }

      [Test]
      public void ReplaceTextBetween_RemovesOuterTokens()
      {
         string s = "This is a test";
         Assert.AreEqual("This unit test", s.ReplaceTagged(" is", "a ", " unit ", true));
      }

      [Test]
      public void ReplaceTextBetween_DoesNotRemoveOuterTokens()
      {
         string s = "This is a test";
         Assert.AreEqual("This is unit a test", s.ReplaceTagged(" is", "a ", " unit ", false));
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
