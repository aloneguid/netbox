namespace NetBox.FileFormats.Csv {
    static class CsvFormat {
        public const char ValueSeparator = ',';
        public const char ValueQuote = '"';
        public static readonly string ValueQuoteStr = "\"";
        public static readonly string ValueQuoteStrStr = "\"\"";
        private static readonly char[] QuoteMark = new[] { ValueSeparator, ValueQuote, '\r', '\n' };

        public static readonly char[] NewLine = { '\r', '\n' };

        private const string ValueLeftBracket = "\"";
        private const string ValueRightBracket = "\"";

        private const string ValueEscapeFind = "\"";
        private const string ValueEscapeValue = "\"\"";

        /// <summary>
        /// Implemented according to RFC4180 http://tools.ietf.org/html/rfc4180
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EscapeValue(string value) {
            if(string.IsNullOrEmpty(value)) {
                return string.Empty;
            }

            //the values have to be quoted if they contain either quotes themselves,
            //value separators, or newline characters
            if(value.IndexOfAny(QuoteMark) == -1) {
                return value;
            }

            return ValueQuoteStr +
               value
                  .Replace(ValueQuoteStr, ValueQuoteStrStr)
                  .Replace("\r\n", "\r")
                  .Replace("\n", "\r") +
               ValueQuoteStr;
        }

        public static string? UnescapeValue(string value) {
            if(value == null)
                return null;

            return value;
        }
    }
}