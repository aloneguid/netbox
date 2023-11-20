namespace NetBox.FileFormats.Ini {
    using Xunit;

    public class StructuredIniFileTest {
        private const string OneSectionIni = @"[DEFAULT]
host = thehost
token = thetoken";

        private const string BiggerIni = @"
key = value

[DEFAULT]
host = thehost
token = thetoken

[user1]
host = host2
secret = secret2";


        [Fact]
        public void ReadOneSectionIni() {
            StructuredIniFile ini = StructuredIniFile.FromString(OneSectionIni);

            Assert.Equal("thehost", ini["DEFAULT.host"]);
            Assert.Equal("thetoken", ini["DEFAULT.token"]);
        }

        [Fact]
        public void ListSections() {
            StructuredIniFile ini = StructuredIniFile.FromString(BiggerIni);

            Assert.Equal(new[] { "DEFAULT", "user1" }, ini.SectionNames);

        }

        [Fact]
        public void ListSectionKeys() {
            StructuredIniFile ini = StructuredIniFile.FromString(BiggerIni);

            Assert.Equal(new[] { "host", "token" }, ini.GetSectionKeys("DEFAULT"));
            Assert.Equal(new[] { "host", "secret" }, ini.GetSectionKeys("user1"));

        }
    }
}
