using Xunit;

namespace NetBox.Tests
{
   public class JsonTest
   {
      [Theory]
      [InlineData("simple", "simple")]
      [InlineData(null, null)]
      [InlineData("simp\tle", "simp\\tle")]
      public void ToEscapedJavaScriptString(string input, string expected)
      {
         string escaped = Json.ToEscapedString(input);

         Assert.Equal(expected, escaped);
      }

      [Fact]
      public void Flatten()
      {
         string doc = @"
{
   ""p1"": ""v1"",
   ""obj"":
   {
      ""p2"" : ""v2"",
      ""p3"" : ""v3"",
      ""obj1"" :
      {
         ""p4"": 4
      }
   }
}
";

         string flat = Json.Flatten(doc, true);

         Assert.Equal(@"{""p1"":""v1"",""obj.p2"":""v2"",""obj.p3"":""v3"",""obj.obj1.p4"":4}", flat);
      }
   }
}
