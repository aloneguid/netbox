using Xunit;

namespace NetBox.Tests
{
   public class JavaScriptUtilsTest
   {
      [Theory]
      [InlineData("simple", "simple")]
      [InlineData(null, null)]
      [InlineData("simp\tle", "simp\\tle")]
      public void ToEscapedJavaScriptString(string input, string expected)
      {
         string escaped = JavaScriptUtils.ToEscapedJavaScriptString(input);

         Assert.Equal(expected, escaped);
      }
   }
}
