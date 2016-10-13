#.NET Support Library [![Visual Studio Team services](https://img.shields.io/vso/build/aloneguid/323c5f4c-c814-452d-9eaf-1006c83fd44c/1.svg?maxAge=2592000?style=flat-square)]() [![NuGet](https://img.shields.io/nuget/v/netbox.svg?maxAge=2592000?style=flat-square)]()

A set of useful (possibly) addition to .NET core platform improving the daily productivity. Available for in `.NET 4.5` and `.NET Standard 1.1` builds.

# Extension Methods

## Assembly Extensions

`Version FileVersion()` - gets the assembly file version (i.e. the one set by [assembly:FileVersion] attribute or the one you see by right-clicking assembly properties in Windows Explorer)

`Version ProductVersion()` - gets the product version (set by [assembly:Version] attribute)

`Stream GetSameFolderEmbeddedResourceFile<TTypeNextToFile>(string fileName)` - Reads embedded resource file which lies next to a type specified in `TTypeNextToFile`.

`string GetSameFolderEmbeddedResourceFileAsText<TTypeNextToFile>(string fileName)` - Reads embedded resource file as text, the `TTypeNextToFile` is a type lying in the same folder as embedded resource file.

`string[] GetSameFolderEmbeddedResourceFileAsLines<TTypeNextToFile>(string fileName)` - Reads embedded resource file as array of lines, the `TTypeNextToFile` is a type lying in the same folder as embedded resource file.

## `System.Guid` extensions

`string ToShortest()` - coverts guid to shortest possible representation i.e. 20 characters instead of 36 comparing to the built-in `ToString()` method. To convert back use `Guid FromShortestGuid()` in string extensions.

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

## IDictionary extensions
`void AddRange<TKey, TValue>(IDictionary<TKey, TValue> source)` - adds all elements to the dictionary from `source`

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
- RipeMd160 (`.NET 4.5` only)

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

`kickass_easley`
`amazing_carson`

`string GeneratePersonFirstName()` - Generates a random person first name from an embedded database of over 5'000 first names

`static string GeneratePersonLastName()` - Generates a random person last name from an embedded database of over 8'000 last names

`string GeneratePersonFullName()` - Generates a random person name in the format of FirstName LastName

# `PasswordGenerator` class.
Generates random password, which complies with the strong password rules and does not contain ambiguous characters. Uses crypto API to achieve true randomness.

`string Generate()` - generates a random password using default password policy.
`string Generate(PasswordPolicy policy)` - generates a random password using a custom policy. You can create the policy by instantiating the `PasswordPolicy` class and changing the allowed character sets etc.

# `NetPath` class

Created due to `System.IO.Path` not being extensible. Contains two static properties:

`static string ExecDir` and `DirectoryInfo ExecDirInfo` which return current execution folder by probing:

- Current Assembly location
- System.Environment property

which is more reliable way to determine current execution folder, especially for windows services.

# `Generator` class

Contains various utilities to generate random data. Uses  [System.Security.Cryptography.RandomNumberGenerator](https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k(System.Security.Cryptography.RandomNumberGenerator);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true) approach. Using crypto calls ensure greater and true randomness.

The class includes a set of static properties and methods for various needs. Properties generate a random value between minimum possible and maximum possible range of values for specific type and include these:

- ` bool RandomBool`
- `long RandomLong`
- `int RandomInt`
- `double RandomDouble`
- `DateTime RandomDate`
- `string RandomString`
- `Uri RandomUri`
- `Enum RandomEmum`
- `T RandomEnum<T>() where T : struct` - `.NET 4.5` only

Properties allow to specify allowed range of values:

- `int GetRandomInt(int max)`
- `int GetRandomInt(int min, int max)`
- `long GetRandomLong(long min, long max)`
- `Enum RandomEnum(Type t)`
- `T RandomEnum<T>() where T : struct` - not available in Portable version
- `DateTime GetRandomDate(DateTime minValue, DateTime maxValue)`
- `string GetRandomString(int length, bool allowNulls)`
- `Uri GetRandomUri(bool allowNulls)`
- `byte[] GetRandomBytes(int minSize, int maxSize)`

todo: ObjectPool, CSV.