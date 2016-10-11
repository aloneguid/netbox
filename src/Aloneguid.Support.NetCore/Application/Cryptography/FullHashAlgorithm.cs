#if NETFULL || NETSTANDARD || NETCORE
using System;
using System.IO;
using System.Security.Cryptography;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Application.Cryptography
{
   class FullHashAlgorithm : IHashAlgorithm
   {
      private readonly HashAlgorithm _native;

      public FullHashAlgorithm(HashType hashType)
      {
         _native = CreateNative(hashType);
      }

      private static HashAlgorithm CreateNative(HashType hashType)
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
#if NETFULL
            case HashType.RipeMd160:
               return RIPEMD160.Create();
#endif
            default:
               throw new NotSupportedException(hashType.ToString() + " is not supported");
         }
      }

      public void Dispose()
      {
         _native.Dispose();
      }

      public byte[] ComputeHash(byte[] buffer)
      {
         return _native.ComputeHash(buffer);
      }

      public byte[] ComputeHash(Stream stream)
      {
         return _native.ComputeHash(stream);
      }
   }
}
#endif