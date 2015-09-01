using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Application
{
   static class Hashing
   {
      private static readonly Dictionary<HashType, HashAlgorithm> Hashers = new Dictionary<HashType, HashAlgorithm>();
      private const int BufferSize = 1024;

      public static byte[] GetHash(byte[] input, HashType hashType)
      {
         if (input == null) return null;
         if (input.Length == 0) return input;

         lock (Hashers)
         {
            return GetHasher(hashType).ComputeHash(input);
         }
      }

      private static HashAlgorithm GetHasher(HashType hashType)
      {
         HashAlgorithm result;
         if (!Hashers.TryGetValue(hashType, out result))
         {
            result = Hashers[hashType] = CreateHasher(hashType);
         }

         return result;
      }

      private static HashAlgorithm CreateHasher(HashType hashType)
      {
         switch (hashType)
         {
            case HashType.Md5:
#if PORTABLE
               ThrowPortableNotSupported(HashType.Md5);
               return null;
#else
               return MD5.Create();
#endif
            case HashType.Sha1:
#if PORTABLE
               return new SHA1Managed();
#else
               return SHA1.Create();
#endif
            case HashType.Sha256:
#if PORTABLE
               return new SHA256Managed();
#else
               return SHA256.Create();
#endif
            case HashType.Sha384:
#if PORTABLE
               ThrowPortableNotSupported(HashType.Sha384);
               return null;
#else
               return SHA384.Create();
#endif
            case HashType.Sha512:
#if PORTABLE
               ThrowPortableNotSupported(HashType.Sha512);
               return null;
#else
               return SHA512.Create();
#endif
            case HashType.RipeMd160:
#if PORTABLE
               ThrowPortableNotSupported(HashType.RipeMd160);
               return null;
#else
               return RIPEMD160.Create();
#endif
            default:
               throw new ArgumentException(hashType + " not supported", nameof(hashType));
         }
      }

      private static void ThrowPortableNotSupported(HashType hashType)
      {
         throw new NotSupportedException(hashType + " is not supported in portable implementation");
      }

#if PORTABLE
      public static byte[][] CalculateHashes(Stream stream, out long streamLength, params HashType[] hashTypes)
      {
         //todo: TransformBlock is not supported in PCL hashes, do something about it later!
         throw new NotImplementedException();
      }
#else
      public static byte[][] CalculateHashes(Stream stream, out long streamLength, params HashType[] hashTypes)
      {
         streamLength = 0;

         if (stream == null || hashTypes == null || hashTypes.Length == 0) return null;

         byte[][] result = new byte[hashTypes.Length][];
         byte[] buffer = new byte[BufferSize];
         byte[][] buffers = new byte[hashTypes.Length][];
         var hashers = new HashAlgorithm[hashTypes.Length];

         for (int i = 0; i < hashTypes.Length; i++)
         {
            hashers[i] = CreateHasher(hashTypes[i]);
            buffers[i] = new byte[BufferSize];
         }

         try
         {
            int read;
            while ((read = stream.Read(buffer, 0, BufferSize)) > 0)
            {
               streamLength += read;
               for (int i = 0; i < hashTypes.Length; i++)
               {
                  hashers[i].TransformBlock(buffer, 0, read, buffers[i], 0);
               }
            }

            //final block
            for (int i = 0; i < hashTypes.Length; i++)
            {
               hashers[i].TransformFinalBlock(buffers[i], 0, read);
               result[i] = hashers[i].Hash;
            }
         }
         finally
         {
            foreach (HashAlgorithm hasher in hashers)
            {
               hasher.Dispose();
            }
         }

         return result;
      }
#endif

   }
}
