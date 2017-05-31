using NetBox.Serialization;
using System.Collections.Generic;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class DictionarySerializerTest
   {
      [Fact]
      public void SerializeDeserialize_TestClass_TestClass()
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

         //check serialization is successful
         Assert.Equal(4, result.Count);
         Assert.Equal("1", result[nameof(TestClass.PublicPropertyWithGetterAndSetter)]);
         Assert.Equal("2", result[nameof(TestClass.PublicMember)]);

         var inline1 = (Dictionary<string, object>)result[nameof(TestClass.Inline1)];
         Assert.Equal("3", inline1[nameof(TestClass1.PublicMember1)]);

         //check it can be deserialized
         object testClassObj = sr.Deserialize(typeof(TestClass), result);
         var testClass = testClassObj as TestClass;
         Assert.Equal("1", testClass.PublicPropertyWithGetterAndSetter);
         Assert.Equal("2", testClass.PublicMember);
         Assert.NotNull(testClass.Inline1);
         Assert.Equal("3", testClass.Inline1.PublicMember1);
      }

      [Fact]
      public void Deserializing_from_changed_type_works_as_long_as_its_convertible()
      {
         var tc = new TestClass
         {
            PublicPropertyWithGetterAndSetter = "1",
            Integer = 5
         };

         var sr = new DictionarySerializer();
         Dictionary<string, object> result = sr.Serialize(tc);

         Assert.Equal(3, result.Count);

         result["Integer"] = "5";

         TestClass tc1 = (TestClass)sr.Deserialize(typeof(TestClass), result);
         Assert.Equal(5, tc1.Integer);
      }
   }

   class TestClass
   {
      public string PublicPropertyWithGetterAndSetter { get; set; }

      public string PublicMember;

      public int Integer;

      public TestClass1 Inline1 { get; set; }

   }

   class TestClass1
   {
      public string PublicMember1;
   }
}
