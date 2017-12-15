# Library NetBox

## Namespaces

 - [NetBox](##NetBox) *(3)*
 - [NetBox.Application](##NetBox.Application) *(2)*
 - [NetBox.Caching](##NetBox.Caching) *(1)*
 - [NetBox.Collections](##NetBox.Collections) *(1)*
 - [NetBox.Data](##NetBox.Data) *(1)*
 - [NetBox.Extensions](##NetBox.Extensions) *(18)*
 - [NetBox.FileFormats](##NetBox.FileFormats) *(2)*
 - [NetBox.Generator](##NetBox.Generator) *(3)*
 - [NetBox.IO](##NetBox.IO) *(3)*
 - [NetBox.Model](##NetBox.Model) *(1)*
 - [NetBox.Performance](##NetBox.Performance) *(1)*

## NetBox

### G

Global defaults

#### Fields

|Name|Summary|
|----|-------|
|C|Global default culture|
|Enc|Global default encoding|
#### Properties

|Name|Summary|
|----|-------|
|LowercaseCharacters|List of allowed lowercase characters|
|MaxLength|Maximum length of the password|
|MinLength|Minimum length of the password|
|NumericCharacters|List of allowed numeric characters|
|RandomBool|Generates a random boolean|
|RandomDate|Generates a random date value|
|RandomDouble|Returns random double|
|RandomInt|Generates a random integer between 0 and max|
|RandomLong|Generates a random long number between 0 and max|
|RandomString|Generates a random string. Never returns null.|
|RandomUri|Generates a random URL in format "http://random.com/random.random. Never returns null values.|
|SpecialCharacters|List of allowed special characters|
|UppercaseCharacters|List of allowed uppercase characters|
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.Int32 minLength, System.Int32 maxLength)|Constructs an intance|
|Generate()|Generates a random password using default password policy|
|Generate(NetBox.Generator.PasswordPolicy policy)|Generates a random password.|
|GetRandomBytes(System.Int32, System.Int32)|Generates a random sequence of bytes of a specified size|
|GetRandomDate(System.DateTime minValue, System.DateTime maxValue)|Generates a random date in range|
|GetRandomInt(System.Int32 max)|Generates a random integer until max parameter|
|GetRandomInt(System.Int32 min, System.Int32 max)|Generates a random integer number in range|
|GetRandomLong(System.Int64 min, System.Int64 max)|Generates a random long number in range|
|GetRandomString(System.Int32 length, System.Boolean allowNulls)|Generates a random string|
|GetRandomUri(System.Boolean allowNulls)|Generates a random URL in format "http://random.com/random.random|
|RandomEnum(System.Type)|Generates a random enum value by type|
### Json

Common JavaScript helpers

#### Methods

|Name|Summary|
|----|-------|
|ToEscapedString(System.String s)|Escapes a string for JSON encoding|
### HashType

Supported hashing algorithms

#### Fields

|Name|Summary|
|----|-------|
|Md5|500Mb hashes in 1462 ms, size: 16b|
|Sha1|500Mb hashes in 1644 ms, size: 20b|
|Sha256|500Mb hashes in 5618 ms, size: 32b|
|Sha384|500Mb hashes in 3839 ms, size: 48b|
|Sha512|500Mb hashes in 3820 ms, size: 64b|
## NetBox.Application

### Ascii85

C# implementation of ASCII85 encoding. Based on C code from http://www.stillhq.com/cgi-bin/cvsweb/ascii85/

#### Fields

|Name|Summary|
|----|-------|
|LineLength|Maximum line length for encoded ASCII85 string; set to zero for one unbroken line.|
|PrefixMark|Prefix mark that identifies an encoded ASCII85 string|
|SuffixMark|Suffix mark that identifies an encoded ASCII85 string|
#### Methods

|Name|Summary|
|----|-------|
|Decode(System.String s, System.Boolean enforceMarks)|Decodes an ASCII85 encoded string into the original binary data|
|Encode(System.Byte[] ba, System.Boolean enforceMarks)|Encodes binary data into a plaintext ASCII85 format string|
### WebUtility

This class is ported from .NET 4.6 to support URL encoding/decoding functionality which is missing in .NET Standard

## NetBox.Caching

### LazyVar&lt;T&gt;

Implements a lazy value i.e. that can expire in future

 - **T** - *Type of an object to wrap in*
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.TimeSpan timeToLive, System.Func{System.Threading.Tasks.Task{`0}} renewFunc)|Creates an instance of a lazy variable with time-to-live value|
|#ctor(System.TimeSpan timeToLive, System.Func{`0} renewFunc)|Creates an instance of a lazy variable with time-to-live value|
|GetValue()|Gets the values, renewing it if necessary|
|GetValueAsync()|Gets the values, renewing it if necessary|
## NetBox.Collections

### ObjectPool&lt;T&gt;

Implement an object pool which allows object to be reusable and cached for a specific amount of time. This class is thread safe.

 - **T** - *Type of object to be pooled*
#### Properties

|Name|Summary|
|----|-------|
|HasFreeInstance|Returns true if object pool has instances in the cache which have not been used yet.|
|InstanceCount|Returns the number of instances in use|
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.Func{`0} createFunction, System.Action{`0} destroyFunction, System.Int32 capacity, System.TimeSpan maxAge)||
|Dispose()|Releases all object instances and calls the destroy function on each of them if you have specified one.|
|GetInstance()|Gets an available instance from the object pool by creating a new one, reusing existing one, or waiting for an available one.|
|ReleaseInstance(`0)|Returns instance to the object pool so it can be reused|
## NetBox.Data

### DynamicValue

Represents a primitive value which can be constructed from anything and converted to anything. "Anything" is to the best effort and will be extended constantly.

#### Properties

|Name|Summary|
|----|-------|
|OriginalType|Original type|
|OriginalValue|Original value|
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.Object value)|Constructs an instance of DynamicValue|
|GetValue(System.Type t)|Gets value of specified type|
|GetValue``1()|Gets value of specified type|
|op_Implicit(NetBox.Data.DynamicValue)~System.Int32)|Implicitly converts to int data type|
|op_Implicit(System.Int32)~NetBox.Data.DynamicValue)|Implicitly converts from int to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.UInt32)|Implicitly converts to uint data type|
|op_Implicit(System.UInt32)~NetBox.Data.DynamicValue)|Implicitly converts from uint to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Int64)|Implicitly converts to long data type|
|op_Implicit(System.Int64)~NetBox.Data.DynamicValue)|Implicitly converts from long to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.UInt64)|Implicitly converts to ulong data type|
|op_Implicit(System.UInt64)~NetBox.Data.DynamicValue)|Implicitly converts from ulong to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Double)|Implicitly converts to double data type|
|op_Implicit(System.Double)~NetBox.Data.DynamicValue)|Implicitly converts from double to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Guid)|Implicitly converts to Guid data type|
|op_Implicit(System.Guid)~NetBox.Data.DynamicValue)|Implicitly converts from Guid to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.DateTime)|Implicitly converts to DateTime data type|
|op_Implicit(System.DateTime)~NetBox.Data.DynamicValue)|Implicitly converts from DateTime to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.DateTimeOffset)|Implicitly converts to DateTimeOffset data type|
|op_Implicit(System.DateTimeOffset)~NetBox.Data.DynamicValue)|Implicitly converts from DateTimeOffset to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.String)|Implicitly converts to string data type|
|op_Implicit(System.String)~NetBox.Data.DynamicValue)|Implicitly converts from string to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Byte[])|Implicitly converts to byte[] data type|
|op_Implicit(System.Byte[])~NetBox.Data.DynamicValue)|Implicitly converts from byte[] to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Boolean)|Implicitly converts to bool data type|
|op_Implicit(System.Boolean)~NetBox.Data.DynamicValue)|Implicitly converts from bool to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Byte)|Implicitly converts to byte data type|
|op_Implicit(System.Byte)~NetBox.Data.DynamicValue)|Implicitly converts from byte to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.SByte)|Implicitly converts to sbyte data type|
|op_Implicit(System.SByte)~NetBox.Data.DynamicValue)|Implicitly converts from sbyte to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Char)|Implicitly converts to char data type|
|op_Implicit(System.Char)~NetBox.Data.DynamicValue)|Implicitly converts from char to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Decimal)|Implicitly converts to decimal data type|
|op_Implicit(System.Decimal)~NetBox.Data.DynamicValue)|Implicitly converts from decimal to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Single)|Implicitly converts to float data type|
|op_Implicit(System.Single)~NetBox.Data.DynamicValue)|Implicitly converts from float to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.Int16)|Implicitly converts to short data type|
|op_Implicit(System.Int16)~NetBox.Data.DynamicValue)|Implicitly converts from short to an instance of DynamicValue|
|op_Implicit(NetBox.Data.DynamicValue)~System.UInt16)|Implicitly converts to ushort data type|
|op_Implicit(System.UInt16)~NetBox.Data.DynamicValue)|Implicitly converts from ushort to an instance of DynamicValue|
## NetBox.Extensions

### AssemblyExtensions

extension methods

#### Methods

|Name|Summary|
|----|-------|
|FileVersion(System.Reflection.Assembly asm)|Gets the file version (set by [assembly:FileVersion] attribute)|
|GetSameFolderEmbeddedResourceFile``1(System.Reflection.Assembly assembly, System.String fileName)|Reads embedded resource file which lies next to a type specified in TTypeNextToFile|
|GetSameFolderEmbeddedResourceFileAsLines``1(System.Reflection.Assembly assembly, System.String fileName)|Reads embedded resource file as array of lines|
|GetSameFolderEmbeddedResourceFileAsText``1(System.Reflection.Assembly assembly, System.String fileName)|Reads embedded resource file as text|
|ProductVersion(System.Reflection.Assembly)|Gets the product version (set by [assembly:Version] attribute)|
### ByteArrayExtensions

Byte array extensions methods

#### Methods

|Name|Summary|
|----|-------|
|GetHash(System.Byte[], NetBox.HashType)|Calculates a hash on byte array|
|Gzip(System.Byte[])|Gzips sequence of bytes|
|IsGzipped(System.Byte[])|Checks the beginning of the byte array for a GZIP header|
|ToHexString(System.Byte[])|Converts byte array to hexadecimal string|
|Ungzip(System.Byte[])|Ungzips sequence of bytes|
### DateTimeExtensions

extension methods

#### Methods

|Name|Summary|
|----|-------|
|EndOfDay(System.DateTime time)|Changes to the end of day time, i.e. hours, minutes and seconds are changed to 23:59:59|
|IsToday(System.DateTime)|Returns true if the date is today's date.|
|IsTomorrow(System.DateTime)|Returns true if the date is tomorrow's date.|
|RoundToDay(System.DateTime)|Strips time from the date structure|
|RoundToMinute(System.DateTime time, System.Int32 round, System.Boolean roundLeft)|Rounds to the closest minute|
|RoundToSecond(System.DateTime time)|Strips off details after seconds|
|ToHourMinuteString(System.DateTime)|Returns date in "HH:mm" format|
|ToIso8601DateString(System.DateTime)|Formats date in ISO 8601 format|
### DictionaryExtensions

IDictionary extensions

#### Methods

|Name|Summary|
|----|-------|
|AddRange``2(System.Collections.Generic.IDictionary{``0 source, ``1} TKey, System.Collections.Generic.IDictionary{``0 target, ``1} TValue)|Adds all elements from source to target|
|GetOrAdd``2(System.Collections.Generic.IDictionary{``0 target, ``1} TKey, ``0 TValue, System.Func{``1} key)|Gets element by key if it exists in the dictionary, otherwise calls specifed method to create a new element and adds it back to the dictionary|
### EnumerableEx

Enumerable extras

#### Methods

|Name|Summary|
|----|-------|
|AddRange``1(System.Collections.Generic.ICollection{``0} collection, System.Collections.Generic.IEnumerable{``0} source)|ICollection extension brining the useful AddRange from List|
|Chunk``1(System.Collections.Generic.IEnumerable{``0} source, System.Int32 chunkSize)|Split sequence in batches of specified size|
|ForEach``1(System.Collections.Generic.IEnumerable{``0}, System.Action{``0})|Performs a specific action on each element of the sequence|
|InferType(System.String[] data)|Tries to guess the type of data contained in the strings|
|InferType(System.String[] data, *out* System.Collections.IList values)|Tries to guess the type of data contained in the strings|
|MultiIterate``2(System.Collections.Generic.IEnumerable{``0} first, System.Collections.Generic.IEnumerable{``1} second)|Iterates over two until one of them reaches the end of elements|
### EnumerableExtensions

extension methods

#### Methods

|Name|Summary|
|----|-------|
|AddRange``1(System.Collections.Generic.ICollection{``0} collection, System.Collections.Generic.IEnumerable{``0} source)|ICollection extension brining the useful AddRange from List|
|Chunk``1(System.Collections.Generic.IEnumerable{``0} source, System.Int32 chunkSize)|Split sequence in batches of specified size|
|ForEach``1(System.Collections.Generic.IEnumerable{``0}, System.Action{``0})|Performs a specific action on each element of the sequence|
|InferType(System.String[] data)|Tries to guess the type of data contained in the strings|
|InferType(System.String[] data, *out* System.Collections.IList values)|Tries to guess the type of data contained in the strings|
### EnumExtensions

Enum extensions methods

### GuidExtensions

Guid extensions

#### Methods

|Name|Summary|
|----|-------|
|ToShortest(System.Guid)|Returns shortest possible representation of guid as string i.e. 20 characters instead of 36 comparing to built-int ToString() representation. It uses ASCII86 encoding to achieve this. See to convert back to Guid.|
### IntExtensions

extension methods

#### Methods

|Name|Summary|
|----|-------|
|Hours(System.Int32 number)|Converts number to hours|
|Minutes(System.Int32 number)|Converts number to minutes|
|Seconds(System.Int32 number)|Converts number to seconds|
|ToFileSizeString(System.Int32)|Converts number to readable size string in IEC format, i.e. 1024 converts to "1.02 KiB"|
|ToFileSizeUiString(System.Int32)|Converts number to readable size string in SI format, i.e. 1024 converts to "1.02 KB"|
### ListExtensions

Extensions methods for and

#### Methods

|Name|Summary|
|----|-------|
|AddRange(System.Collections.IList destination, System.Collections.IEnumerable source)|Adds range of elements into the non-generic list|
|AddRange(System.Collections.IList destination, System.Collections.ICollection source)|Adds range of elements into the non-generic list|
### LongExtensions

extension methods

#### Methods

|Name|Summary|
|----|-------|
|ToFileSizeString(System.Int64)|Converts number to readable size string in IEC format, i.e. 1024 converts to "1.02 KiB"|
|ToFileSizeUiString(System.Int64)|Converts number to readable size string in SI format, i.e. 1024 converts to "1.02 KB"|
### NetFile

extensions. Due to the fact is a static class and cannot be extended with extension methods this is implemented a new static class.

#### Methods

|Name|Summary|
|----|-------|
|Gzip(System.String filePath, System.String targetPath)|Gzips the file|
|Ungzip(System.String filePath, System.String targetPath)|Ungzips the file|
### NetPath

extensions. Due to the fact is a static class and cannot be extended with extension methods this is implemented a new static class.

#### Properties

|Name|Summary|
|----|-------|
|ExecDir|Gets current assembly execution directory in a more reliable way|
|ExecDirInfo|Gets current assembly execution directory information in a more reliable way|
### NetworkCredentialExtensions

Provides extension methods for

#### Methods

|Name|Summary|
|----|-------|
|ToFriendlyString(System.Net.NetworkCredential credential)|Converts to a friendly format which is username:password@domain|
### ObjectExtensions

Object extensions.

#### Methods

|Name|Summary|
|----|-------|
|AsEnumerable``1(``0 T)|Creates an enumerable from one element|
### StreamExtensions

extension

#### Methods

|Name|Summary|
|----|-------|
|GetHash(System.IO.Stream stream, NetBox.HashType hashType)|Calculates hash on input stream.|
|GetHashBytes(System.IO.Stream stream, NetBox.HashType hashType)|Calculates hash on input stream.|
|Gzip(System.IO.Stream, System.IO.Stream)|GZips source stream into a target stream|
|ReadUntil(System.IO.Stream, System.Byte[])|Reads the stream until a specified sequence of bytes is reached.|
|ToByteArray(System.IO.Stream)|Reads all stream in memory and returns as byte array|
|ToString(System.IO.Stream, System.Text.Encoding)|Converts the stream to string using specified encoding. This is done by reading the stream into byte array first, then applying specified encoding on top.|
|TryGetSize(System.IO.Stream, *out* System.Int64)|Attemps to get the size of this stream by reading the Length property, otherwise returns 0.|
|TryGetSize(System.IO.Stream)|Attemps to get the size of this stream by reading the Length property, otherwise returns 0.|
|Ungzip(System.IO.Stream, System.IO.Stream)|UnGZips source stream into a target stream|
### StringExtensions

String extensions.

#### Methods

|Name|Summary|
|----|-------|
|Base64Decode(System.String)|Decodes a BASE64 encoded string|
|Base64DecodeAsBytes(System.String s)|Decodes a BASE64 encoded string to byte array|
|Base64Encode(System.String)|Encodes a string to BASE64 format|
|Capitalize(System.String)|Transforms the string so that the first letter is uppercase and the rest of them are lowercase|
|FindTagged(System.String s, System.String startTag, System.String endTag, System.Boolean includeOuterTags)|Looks for and followed in sequence and when found returns the text between them.|
|FromHexToBytes(System.String hex)|Convers hex string to byte array|
|FromShortestGuid(System.String)|Converts shortest guid representation back to Guid. See on how to convert Guid to string.|
|GetHash(System.String s, NetBox.HashType hashType)|Gets string hash|
|Gzip(System.String, System.Text.Encoding)|Gzips a specified string into array of bytes using specified encoding|
|Gzip(System.String, System.Text.Encoding, System.IO.Stream)|Gzips a specified string in specified encoding to to destination stream.|
|HtmlDecode(System.String)|Decodes from HTML string|
|HtmlEncode(System.String)|Encodes to HTML string|
|MatchesWildcard(System.String s, System.String wildcard)|Filesystem style widlcard match where * stands for any characters of any length and ? standa for one character|
|PairedWith``1(System.String, ``0)|Creates a key-value pair from this string and an object value|
|ReplaceTagged(System.String s, System.String startTag, System.String endTag, System.String replacementText, System.Boolean replaceOuterTokens)|Looks for and followed in sequence, and if found performs a replacement of text inside them with|
|SanitizePath(System.String)|Removes invalid path characters from the string, replacing them by space (' ') character|
|SanitizePath(System.String, System.Char)|Removes invalid path characters from the string, replacing them by the given character|
|Slice(System.String s, System.Nullable{System.Int32} start, System.Nullable{System.Int32} end)|Pythonic approach to slicing strings|
|SpacedToCamelCase(System.String)|Converts a string with spaces to a camel case version, for example "The camel string" is converted to "TheCamelString"|
|SplitByDelimiter(System.String s, System.String[] delimiter)|Splits the string into key and value using the provided delimiter values. Both key and value are trimmed as well.|
|StripHtml(System.String s)|Strips HTML string from any tags leaving text only.|
|ToMemoryStream(System.String, System.Text.Encoding)|Converts to MemoryStream with a specific encoding|
|ToMemoryStream(System.String)|Converts to MemoryStream in UTF-8 encoding|
|ToNetworkCredential(System.String s)|Converts string to network credentials if it's formatted in a friendly way. See how to do it.|
|UrlDecode(System.String value)|URL-decodes input string|
|UrlEncode(System.String value)|URL-encodes input string|
### TypeExtensions

extensions

#### Methods

|Name|Summary|
|----|-------|
|CreateDefaultValue(System.Type t)|Creates the default value for type. It's null for reference type and 0 for int, float etc.|
|FileVersion(System.Type t)|Gets the file version (set by [assembly:FileVersion] attribute)|
|GetAssembly(System.Type t)|Gets the assembly this type is in|
|GetHierarchyPublicProperties(System.Type type)|Gets public properties of this type and all derived types. Handles both classes and interfaces.|
|GetSameFolderEmbeddedResourceFile(System.Type type, System.String fileName)|Reads embedded resource file which lies next to a type specified in TTypeNextToFile|
|GetSameFolderEmbeddedResourceFileAsLines``1(System.Type type, System.String fileName)|Reads embedded resource file as array of lines|
|GetSameFolderEmbeddedResourceFileAsText(System.Type type, System.String fileName)|Reads embedded resource file as text|
|IsEnum(System.Type)|More reliable way to determine if a type is Enum|
|IsNumber(System.Type t)|Checks if this type is a primitive number, i.e. one of: - byte/sbyte - decimal - double - float - int - long/ulong - short/ushort|
|IsSimple(System.Type t)|Deterimites if type is "simple". It's hard to describe what a simple type is, but this is used mostly in serialization and says whether a type has to be expanded (not simple) or represented as a value.|
|ProductVersion(System.Type)|Gets the product version (set by [assembly:Version] attribute)|
## NetBox.FileFormats

### CsvReader

Reads data from a CSV file. Fast and reliable, supports: - newline characters - double quotes - commas

#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.IO.Stream, System.Text.Encoding)|Creates an instance from an open stream and encoding|
|ReadAllFromContent(System.String content, System.Boolean hasColumns)|Reads all file as a dictionary of column name to list of values|
|ReadNextRow()|Reads next row of data if available.|
### CsvWriter

Writes data to a CSV file. Fast and reliable, supports: - newline characters - double quotes - commas

#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.IO.Stream destination)|Creates a new instance of CsvWriter which uses UTF8 encoding|
|#ctor(System.String fileName)|Creates a new instance of CsvWriter on disk with UTF8 encoding|
|#ctor(System.IO.Stream destination, System.Text.Encoding encoding)|Creates a new instance of CsvWriter and allows to specify the writer encoding|
|Write(System.String[])|Writes a row of data|
|Write(System.Collections.Generic.IEnumerable{System.String})|Writes a row of data|
## NetBox.Generator

### PasswordGenerator

Generates random passwords

#### Methods

|Name|Summary|
|----|-------|
|Generate()|Generates a random password using default password policy|
|Generate(NetBox.Generator.PasswordPolicy policy)|Generates a random password.|
### PasswordPolicy

Password policy used by

#### Properties

|Name|Summary|
|----|-------|
|LowercaseCharacters|List of allowed lowercase characters|
|MaxLength|Maximum length of the password|
|MinLength|Minimum length of the password|
|NumericCharacters|List of allowed numeric characters|
|SpecialCharacters|List of allowed special characters|
|UppercaseCharacters|List of allowed uppercase characters|
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.Int32 minLength, System.Int32 maxLength)|Constructs an intance|
### RandomGenerator

Generates random data using for increased security

#### Properties

|Name|Summary|
|----|-------|
|RandomBool|Generates a random boolean|
|RandomDate|Generates a random date value|
|RandomDouble|Returns random double|
|RandomInt|Generates a random integer between 0 and max|
|RandomLong|Generates a random long number between 0 and max|
|RandomString|Generates a random string. Never returns null.|
|RandomUri|Generates a random URL in format "http://random.com/random.random. Never returns null values.|
#### Methods

|Name|Summary|
|----|-------|
|GetRandomBytes(System.Int32, System.Int32)|Generates a random sequence of bytes of a specified size|
|GetRandomDate(System.DateTime minValue, System.DateTime maxValue)|Generates a random date in range|
|GetRandomInt(System.Int32 max)|Generates a random integer until max parameter|
|GetRandomInt(System.Int32 min, System.Int32 max)|Generates a random integer number in range|
|GetRandomLong(System.Int64 min, System.Int64 max)|Generates a random long number in range|
|GetRandomString(System.Int32 length, System.Boolean allowNulls)|Generates a random string|
|GetRandomUri(System.Boolean allowNulls)|Generates a random URL in format "http://random.com/random.random|
|RandomEnum(System.Type)|Generates a random enum value by type|
## NetBox.IO

### BoundedStream

Works on top of a subportion of a stream, making only a small window visible

### DelegatedStream

Makes stream members virtual instead of abstract, allowing to override only specific behaviors.

#### Properties

|Name|Summary|
|----|-------|
|CanRead|Calls|
|CanSeek|Calls|
|CanWrite|Calls|
|Length|Calls|
|Position|Delegates to master by default|
#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.IO.Stream master)|Creates an instance of non-closeable stream|
|Flush()|Delegates to master by default|
|GetCanRead()|Delegates to master by default|
|GetCanSeek()|Delegates to master by default|
|GetCanWrite()|Delegates to master by default|
|GetLength()|Delegates to master by default|
|Read(System.Byte[], System.Int32, System.Int32)|Delegates to master by default|
|Seek(System.Int64, System.IO.SeekOrigin)|Delegates to master by default|
|SetLength(System.Int64)|Delegates to master by default|
|Write(System.Byte[], System.Int32, System.Int32)|Delegates to master by default|
### NonCloseableStream

Represents a stream that ignores operations i.e. cannot be closed by the client

#### Methods

|Name|Summary|
|----|-------|
|#ctor(System.IO.Stream master)|Creates an instance of this class|
|Dispose(System.Boolean disposing)|Overrides this call to do nothing|
## NetBox.Model

### EnumTagAttribute

Tags enums for assigning extra data such as alternative ID, Description etc.

#### Properties

|Name|Summary|
|----|-------|
|DisplayName|Display name|
|Id|Alternative string version of ID|
#### Methods

|Name|Summary|
|----|-------|
|#ctor()|Creates an instance|
|#ctor(System.String id, System.String displayName)|Creates an instance|
## NetBox.Performance

### Measure

Measures a time slice as precisely as possible

#### Properties

|Name|Summary|
|----|-------|
|ElapsedMilliseconds|Returns number of elapsed milliseconds since the start of measure. The measuring process will continue running.|
|ElapsedTicks|Returns number of elapsed ticks since the start of measure. The measuring process will continue running.|
#### Methods

|Name|Summary|
|----|-------|
|#ctor()|Creates the measure object|
|Dispose()|Stops measure object if still running|
