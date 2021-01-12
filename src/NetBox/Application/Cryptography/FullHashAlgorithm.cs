using System;
using System.IO;
using System.Security.Cryptography;
using NetBox.Model;

namespace NetBox.Application.Cryptography
{
   class FullHashAlgorithm : IHashAlgorithm
   {
      private readonly HashAlgorithm _native;

      public FullHashAlgorithm(HashType hashType, byte[] salt)
      {
         _native = CreateNative(hashType, salt);
      }

      private static HashAlgorithm CreateNative(HashType hashType, byte[] salt)
      {
         switch(hashType)
         {
            case HashType.Md5:
               return MD5.Create();
            case HashType.Sha1:
               return SHA1.Create();
            case HashType.Sha256:
               return SHA256.Create();
            case HashType.Sha384:
               return SHA384.Create();
            case HashType.Sha512:
               return SHA512.Create();
            case HashType.HMACSHA256:
               return new HMACSHA256(salt);
            default:
               throw new NotSupportedException(hashType.ToString() + " is not supported");
         }
      }

      public void Dispose()
      {
         _native.Dispose();
      }

      public byte[] ComputeHash(byte[] buffer, byte[] salt)
      {
         return _native.ComputeHash(buffer);
      }

      public byte[] ComputeHash(Stream stream, byte[] salt)
      {
         return _native.ComputeHash(stream);
      }
   }
}