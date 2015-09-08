extern alias Portable;
using System;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class StringExtensionsTest
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
      [ExpectedException(typeof(InvalidOperationException))]
      public void XmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         ho.XmlSerialise();
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void PortableXmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         Portable::System.ObjectExtensions.XmlSerialise(ho);
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void XmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         nxo.XmlSerialise();
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void PortableXmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         Portable::System.ObjectExtensions.XmlSerialise(nxo);
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
