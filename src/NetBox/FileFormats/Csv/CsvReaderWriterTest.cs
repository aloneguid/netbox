namespace NetBox.FileFormats.Csv {
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Text;
    using global::System.Text.RegularExpressions;
    using NetBox.Performance;
    using Xunit;

    public class CsvReaderWriterTest {
        private readonly CsvWriter _writer;
        private CsvReader _reader;
        private readonly MemoryStream _ms;

        public CsvReaderWriterTest() {
            _ms = new MemoryStream();
            _writer = new CsvWriter(_ms, Encoding.UTF8);
            _reader = new CsvReader(_ms, Encoding.UTF8);
        }

        private void SetReaderFromWriter() {
            _ms.Flush();
            _ms.Position = 0;
            _reader = new CsvReader(_ms, Encoding.UTF8);
        }

        [Fact]
        public void Write_2RowsOfDifferentSize_Succeeds() {
            _writer.Write("11", "12");
            _writer.Write("21", "22", "23");
        }

        [Fact]
        public void Write_2RowsOfSameSize_Succeeds() {
            _writer.Write("11", "12");
            _writer.Write("21", "22");
        }

        [Fact]
        public void Write_NoEscaping_JustQuotes() {
            _writer.Write("1", "-=--=,,**\r\n77$$");

            string result = Encoding.UTF8.GetString(_ms.ToArray());

            Assert.Equal("1,\"-=--=,,**\r77$$\"", result);
        }

        [Fact]
        public void Write_WithEscaping_EscapingAndQuoting() {
            _writer.Write("1", "two of \"these\"");

            string result = Encoding.UTF8.GetString(_ms.ToArray());

            Assert.Equal("1,\"two of \"\"these\"\"\"", result);

        }

        [Fact]
        public void WriteRead_WriteTwoRows_ReadsTwoRows() {
            _writer.Write("r1c1", "r1c2", "r1c3");
            _writer.Write("r2c1", "r2c2");
            SetReaderFromWriter();

            string[]? r1 = _reader.ReadNextRow();
            string[]? r2 = _reader.ReadNextRow();
            string[]? r3 = _reader.ReadNextRow();

            Assert.Null(r3);
            Assert.Equal(2, r2!.Length);
            Assert.Equal(3, r1!.Length);

            Assert.Equal("r2c1", r2[0]);
        }

        [Fact]
        public void WriteRead_Multiline_Succeeds() {
            _writer.Write(@"mu
lt", "nm");
            _writer.Write("1", "2");
            SetReaderFromWriter();

            //validate first row
            string[]? r = _reader.ReadNextRow();
            Assert.Equal(2, r!.Length);
            Assert.Equal(@"mu
lt", r[0], false, true);
            Assert.Equal("nm", r[1]);

            //validate second row
            r = _reader.ReadNextRow();
            Assert.Equal(2, r!.Length);
            Assert.Equal("1", r[0]);
            Assert.Equal("2", r[1]);

            //validate there is no more rows
            Assert.Null(_reader.ReadNextRow());
        }

        [Fact]
        public void WriteRead_OneColumnOneValue_Reads() {
            _writer.Write("RowKey");
            _writer.Write("rk");

            SetReaderFromWriter();

            string[]? header = _reader.ReadNextRow();
            string[]? values = _reader.ReadNextRow();

            Assert.NotNull(header);
            Assert.NotNull(values);

            Assert.Single(header);
            Assert.Equal("RowKey", header[0]);

            Assert.Single(values);
            Assert.Equal("rk", values[0]);
        }

        [Fact]
        public void WriteRead_EmptyUnquotedValue_Included() {
            _writer.Write("one", "", "three");
            SetReaderFromWriter();

            string[]? row = _reader.ReadNextRow();
            Assert.Equal(3, row!.Length);
            Assert.Equal("one", row[0]);
            Assert.Equal("", row[1]);
            Assert.Equal("three", row[2]);
        }

        [Fact]
        public void WriteRead_Case001_Fixed() {
            _writer.Write("RowKey", "col1", "col2", "col3");
            _writer.Write("rk1", "val11", "val12", "");
            _writer.Write("rk2", "", "val22", "val23");

            _ms.Flush();
            _ms.Position = 0;

            _reader = new CsvReader(_ms, Encoding.UTF8);

            string[]? h = _reader.ReadNextRow();
            string[]? r1 = _reader.ReadNextRow();
            string[]? r2 = _reader.ReadNextRow();
            string[]? nl = _reader.ReadNextRow();

            Assert.NotNull(h);
            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.Null(nl);
        }

        [Fact]
        public void Read_all_content_as_dictionary_with_column_names() {
            const string csv = @"col1,col2
1,11
2,22
";

            Dictionary<string, List<string>> f = CsvReader.ReadAllFromContent(csv);

            Assert.Equal(2, f.Count);
            Assert.Equal("col1", f.Keys.First());
            Assert.Equal("col2", f.Keys.Skip(1).First());
        }

        [Fact]
        public void Performance_Escaping_Stands() {
            const string ValueEscapeFind = "\"";
            const string ValueEscapeValue = "\"\"";

            const int loops = 10000;
            const string s = "kjkj\"jfjflj\"\"\"";
            long time1, time2;

            //experiment 1
            using(var m = new TimeMeasure()) {
                for(int i = 0; i < loops; i++) {
                    string s1 = s.Replace(ValueEscapeFind, ValueEscapeValue);
                }

                time1 = m.ElapsedTicks;
            }

            //experiment 2
            var rgx = new Regex("\"", RegexOptions.Compiled);
            using(var m = new TimeMeasure()) {
                for(int i = 0; i < loops; i++) {
                    string s1 = rgx.Replace(s, ValueEscapeValue);
                }

                time2 = m.ElapsedTicks;
            }

            //regex.replace is MUCH slower than string.replace

            Assert.NotEqual(time1, time2);
        }
    }
}