extern alias Portable;
using System;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class StringExtensionsTest
   {
      [Test]
      public void XmlSerialise_Null_Null()
      {
         string s = ((object)null).XmlSerialise();
         Assert.IsNull(s);
      }

      [Test]
      public void PortableXmlSerialise_Null_Null()
      {
         string s = Portable::System.ObjectExtensions.XmlSerialise((object) null);
         Assert.IsNull(s);
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
      [ExpectedException(typeof(InvalidOperationException))]
      public void XmlSerialise_HiddenObject_Fails()
      {
         var ho = new HiddenDoc();
         ho.XmlSerialise();
      }

      [Test]
      [ExpectedException(typeof(InvalidOperationException))]
      public void XmlSerialise_NonXmlObject_Fails()
      {
         var nxo = new NonXmlDoc(5);
         nxo.XmlSerialise();
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
      public void Base64_Encode_Decodes()
      {
         string s = "test string";
         string s64 = s.Base64Encode();
         string s2 = s64.Base64Decode();

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
