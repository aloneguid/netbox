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

#if PORTABLE
      private static HashAlgorithm CreateHasher(HashType hashType)
      {
         switch (hashType)
         {
            //case HashType.Md5:
            //   throw new NotSupportedException();
            case HashType.Sha1:
               return new SHA1Managed();
            case HashType.Sha256:
               return new SHA256Managed();
            //case HashType.Sha384:
            //   throw new NotSupportedException();
            //case HashType.Sha512:
            //   throw new NotSupportedException();
            //case HashType.RipeMd160:
            //   throw new NotSupportedException();
            default:
               throw new ArgumentException(hashType + " not supported", "hashType");
         }
      }

#else
      private static HashAlgorithm CreateHasher(HashType hashType)
      {
         switch (hashType)
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
            case HashType.RipeMd160:
               return RIPEMD160.Create();
            default:
               throw new ArgumentException(hashType + " not supported", "hashType");
         }
      }

#endif

      public static byte[][] CalculateHashes(Stream stream, out long streamLength, params HashType[] hashTypes)
      {
         throw new NotImplementedException();
         //todo: these need to be read in memory first
         /*streamLength = 0;

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

         return result;*/
      }


   }
}
