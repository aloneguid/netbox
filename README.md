#.NET Support Library

A set of useful (possibly) addition to .NET core platform improving the daily productivity.

![](https://aloneguid.visualstudio.com/DefaultCollection/_apis/public/build/definitions/323c5f4c-c814-452d-9eaf-1006c83fd44c/1/badge)

NuGet package: https://www.nuget.org/packages/Aloneguid.Support/

# Extension Methods

## Assembly Extensions

`Version FileVersion()` - gets the assembly file version (i.e. the one set by [assembly:FileVersion] attribute or the one you see by right-clicking assembly properties in Windows Explorer)

`Version ProductVersion()` - gets the product version (set by [assembly:Version] attribute)

## Byte Array Extensions (byte[])

`string ToHexString()` - gets a hexadecimal representation of the byte array, i.e. `new byte[] { 0, 1, 2, 3, 4, 5 }` is transformed to `"000102030405"`

### Compression

These various extensions are GZip compression helpers:

### on `byte[]`

`byte[] Gzip()` - gzips the array.
`byte[] Ungzip()` - ungzips the array.
`bool IsGzipped()` - checks if byte sequence is gzipped by checking if first few bytes are a GZip header.

### on `System.IO.Stream`

`void Gzip(Stream outputStream)` - gzips stream into target stream.
`void Ungzip(Stream outputStream)` - ungzips source stream into a target stream.

### on `string`

`byte[] Gzip(Encoding encoding)` - Gzips a specified string into array of bytes using specified encoding.
`void Gzip(Encoding encoding, Stream destinationStream)` - Gzips a specified string in specified encoding to to destination stream.

### In a separate `NetFile` class

`static void Gzip(string filePath, string targetPath)` - gzips source file by path to a target file by path.
``

## DateTime extensions

`DateTime RoundToDay()` - strips everything except for yearh, month and day.

`DateTime EndOfDay()` - rolls the time to the maximum withing current date, i.e. to 23:59:59

`DateTime RoundToMinute(int round, bool roundLeft)` - rounds date to closest minute, `round` parameter specifies minute i.e. 15, 30, 45 etc.. `roundLeft` - whether to use minimum or maximum value. For example when time is 13:14 and rounding is to every 15 minutes, when this parameter is truethe result it 13:00, otherwise 13:15.

`bool IsToday()` - returns true when date is in today's day.

`bool IsTomorrow()` - returns true when date is in tomorrow's day.

`bool IsThisWeek()` - returns true when date is in this week's interval.

`bool IsThisMonth()` - returns true when date is in this month's interval.

## IEnumerable/ICollection extensions

`IEnumerable<IEnumerable<T>> Chunk<T>(int chunkSize)` - splits `IEnumerable<T>` into equal chunks of `IEnumerable<T>` specified by `chunkSize`.

`IEnumerable<T> ForEach<T>(Action<T> action)` - Performs a specific action on each element of the sequence, returns original source.

`void AddRange<T>(IEnumerable<T> source)` - works on `ICollection<T>` and brings the `.AddRange()` method available in `List<T>` to add a sequence of elements at once.

## Size Formatting

These extension methods are available on both `int` and `long` types, there are two of them:

`string ToFileSizeString()` - converts number to readable size string in IEC format, i.e. 1024 converts to "1.02 KiB".

`string ToFileSizeUiString()` - converts number to readable size string in SI format, i.e. 1024 converts to "1.02 KB".

# Serialization

Various extension methods are available to achive serialization/deserialization in extremely easy way. All extensions are performance optimised.

### on `System.Object`

`string XmlSerialise()` - serialises any object to XML string if possible. Uses .NET framework's `XmlSerializer` to do this.

`string ToJsonString()` - converts any object to a JSON string if possible. Uses Netwonsoft.Json library to achieve this.

`string ToCompressedJsonString()` - idential to previous method, but formats JSON in one string without line breaks.

### on `System.IO.Stream`

`T ReadAsJsonObject<T>(Encoding encoding)` - deserialise stream into a JSON object.

`object ReadAsJsonObject(Encoding encoding, Type t)` - same as above, non-generic version.

### on `string`

`T XmlDeserialise<T>() where T : class, new()` - deserialises object represented as XML string to a real object.

`object XmlDeserialise(Type t)` - same as above, non-generic version.

`T AsJsonObject<T>()` - deserialises object represented as JSON string to a real object.

`object AsJsonObject(Type t)` - same as above, non-generic version.

# Hashing

List of supported hashes, represented by `HashType` enumeration
- Md5
- Sha1
- Sha256
- Sha384
- Sha512
- RipeMd160

Various extension methods are available to support hashing

### on `byte[]`

`byte[] GetHash(HashType hashType)`

### on `System.IO.Stream`

`string[] GetHashes(params HashType[] hashes)` - calculates multiple hashes on stream, this is a very efficient extension which calculates as many hashes as you need in one pass.

`string[] GetHashes(out long streamLength, params HashType[] hashes)` - same as above, but also returns original stream length which may be useful if you need to calculate multiple hashes on forward-only stream and know it's length.

`string GetHash(HashType hashType)` - same as above, shorter version to calculate just one hash.

`string GetHash(out long streamLength, HashType hashType)` - same as above, but calculates stream length.

### on `string`

`string GetHash(HashType hashType)` - get hash on string.

`string GetHash(Encoding encoding, HashType hashType)` - longer version of above, allowing to specify encoding when converting string to byte array internally.

### Notes

When hash functions are returning `string` you should assume that a hexadecimal representation of byte array hash is returned. This is more useful than returning `byte[]` in most practical applications.

# Name Generator

`string NameGenerator.GenerateDockerStyleName()` generates a funny sb-random name in docker style, the source code for this is migrated from the original [Go source](https://raw.githubusercontent.com/docker/docker/master/pkg/namesgenerator/names-generator.go). Example output:

kickass_easley
amazing_carson



todo: NetPath, Generator, ObjectPool, CSV.