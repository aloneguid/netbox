using Xunit;
using NetBox.Extensions;

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
         string escaped = input.ToEscapedJsonValueString();

         Assert.Equal(expected, escaped);
      }
   }
}
