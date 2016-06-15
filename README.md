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

`GetHash(HashType hashType` - Calculates a hash on byte array, see appendix for list of supported hashes

### Compression

These three extensions are GZip compression helpers:

`byte[] Gzip()` - gzips the array, `byte[] Ungzip()` - ungzips the array, `bool IsGzipped()` - checks if byte sequence is gzipped by checking if first few bytes are a GZip header.

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



# Appendix

## List of supported hashes

This is represented by `HashType` enumeration
- Md5
- Sha1
- Sha256
- Sha384
- Sha512
- RipeMd160


more documentation coming soon...