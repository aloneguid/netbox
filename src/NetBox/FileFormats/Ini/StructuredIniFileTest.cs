namespace NetBox.FileFormats.Ini
{
   using Xunit;

   public class StructuredIniFileTest
   {
      private const string OneSectionIni = @"[DEFAULT]
host = thehost
token = thetoken";

      [Fact]
      public void ReadOneSectionIni()
      {
         StructuredIniFile ini = StructuredIniFile.FromString(OneSectionIni);

         Assert.Equal("thehost", ini["DEFAULT.host"]);
         Assert.Equal("thetoken", ini["DEFAULT.token"]);
      }
   }
}
