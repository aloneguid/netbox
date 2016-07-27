//extern alias Portable;
using System;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class ObjectExtensionsTest
   {
      [Test]
      public void XmlSerialise_Null_Null()
      {
         string s = ((object)null).XmlSerialise();
         Assert.IsNull(s);
      }

      /*[Test]
      public void PortableXmlSerialise_Null_Null()
      {
         string s = Portable::System.ObjectExtensions.XmlSerialise((object)null);
         Assert.IsNull(s);
      }*/

      [Test]
      public void AsJsonObject_InvalidString_Null()
      {
         Tag tag = "test".AsJsonObject<Tag>();

         Assert.IsNull(tag);
      }

      private class Tag
      {
         public string Name { get; set; }
      }
   }
}
