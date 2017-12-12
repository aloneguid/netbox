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
   }
}
