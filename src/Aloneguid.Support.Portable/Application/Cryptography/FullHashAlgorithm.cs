#if !PORTABLE
using System;
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
         switch (hashType)
         {
            case HashType.Md5:
               return MD5.Create();
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

      public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
      {
         return _native.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
      }

      public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
      {
         return _native.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
      }

      public byte[] Hash => _native.Hash;
   }
}
#endif