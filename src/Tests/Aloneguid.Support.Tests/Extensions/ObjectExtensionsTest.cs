//extern alias Portable;
using System;
using Xunit;

namespace Aloneguid.Support.Tests.Extensions
{
   
   public class ObjectExtensionsTest
   {
      [Fact]
      public void XmlSerialise_Null_Null()
      {
         string s = ((object)null).XmlSerialise();
         Assert.Null(s);
      }

      /*[Fact]
      public void PortableXmlSerialise_Null_Null()
      {
         string s = Portable::System.ObjectExtensions.XmlSerialise((object)null);
         Assert.Null(s);
      }*/

      [Fact]
      public void AsJsonObject_InvalidString_Null()
      {
         Tag tag = "test".AsJsonObject<Tag>();

         Assert.Null(tag);
      }

      private class Tag
      {
         public string Name { get; set; }
      }
   }
}
