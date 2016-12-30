using NetBox.Serialization;
using System.Collections.Generic;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class DictionarySerializerTest
   {
      [Fact]
      public void TestClass_Serializes()
      {
         var tc = new TestClass
         {
            PublicPropertyWithGetterAndSetter = "1",
            PublicMember = "2",
            Inline1 = new TestClass1
            {
               PublicMember1 = "3"
            }
         };

         var sr = new DictionarySerializer();
         Dictionary<string, object> result = sr.Serialize(tc);

         Assert.Equal(3, result.Count);
         Assert.Equal("1", result[nameof(TestClass.PublicPropertyWithGetterAndSetter)]);
         Assert.Equal("2", result[nameof(TestClass.PublicMember)]);
         Assert.Equal("3", result[nameof(TestClass1.PublicMember1)]);
      }
   }

   class TestClass
   {
      public string PublicPropertyWithGetterAndSetter { get; set; }

      public string PublicMember;

      public TestClass1 Inline1 { get; set; }

   }

   class TestClass1
   {
      public string PublicMember1;
   }
}
