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


      private class Tag
      {
         public string Name { get; set; }
      }
   }
}
