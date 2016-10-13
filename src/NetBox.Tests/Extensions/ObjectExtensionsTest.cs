//extern alias Portable;
using System;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class ObjectExtensionsTest
   {
#if NETFULL
      [Fact]
      public void XmlSerialise_Null_Null()
      {
         string s = ((object)null).XmlSerialise();
         Assert.Null(s);
      }
#endif

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
