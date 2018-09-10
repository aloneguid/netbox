using System.Collections.Generic;
using NetBox.Extensions;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class DictionaryExtensionsTest
   {
      [Fact]
      public void AddRange_AnotherDictionary_Adds()
      {
         var d1 = new Dictionary<string, string>();
         d1.Add("1", "v1");

         var d2 = new Dictionary<string, string>();
         d2.Add("2", "v2");

         d1.AddRange(d2);

         Assert.True(d1.ContainsKey("2"));
      }

      [Fact]
      public void GetOrAdd_Exists_Returns()
      {
         var d1 = new Dictionary<string, string>();
         d1["one"] = "value";

         string one = d1.GetOrAdd("one", () => "value");
      }

      [Fact]
      public void GetOrAdd_DoesNotExist_CreatesAndReturns()
      {
         var d1 = new Dictionary<string, string>();
         string one = d1.GetOrAdd("one", () => "value");

         Assert.Equal("value", one);
         Assert.Equal("value", d1["one"]);

      }

      [Fact]
      public void Serialise_deserialise_dictionary()
      {
         var d = new Dictionary<string, object>();
         d["one"] = 1;
         d["two"] = "2";

         string json = d.JsonSerialise();

         IDictionary<string, object> d2 = json.JsonDeserialiseDictionary();
         Assert.Equal(2, d2.Count);
         Assert.Equal(1L, d2["one"]);
         Assert.Equal("2", d2["two"]);
      }
   }
}
