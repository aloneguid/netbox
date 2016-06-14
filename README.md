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