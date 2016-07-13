using System;
using System.Collections.Generic;
using System.IO;
using Aloneguid.Support.Application.Cryptography;
using Aloneguid.Support.Model;

namespace Aloneguid.Support.Application
{
   static class Hashing
   {
      private static readonly Dictionary<HashType, IHashAlgorithm> Hashers = new Dictionary<HashType, IHashAlgorithm>();
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

      private static IHashAlgorithm GetHasher(HashType hashType)
      {
         IHashAlgorithm result;
         if (!Hashers.TryGetValue(hashType, out result))
         {
            result = Hashers[hashType] = CreateHasher(hashType);
         }

         return result;
      }

      private static IHashAlgorithm CreateHasher(HashType hashType)
      {
#if PORTABLE
         return new ReflectedHashAlgorithm(hashType);
#endif

#if NETFULL
         return new FullHashAlgorithm(hashType);
#endif

#if NETCORE
         throw new NotImplementedException();
#endif
      }

      public static byte[][] CalculateHashes(Stream stream, out long streamLength, params HashType[] hashTypes)
      {
         streamLength = 0;

         if (stream == null || hashTypes == null || hashTypes.Length == 0) return null;

         byte[][] result = new byte[hashTypes.Length][];
         byte[] buffer = new byte[BufferSize];
         byte[][] buffers = new byte[hashTypes.Length][];
         var hashers = new IHashAlgorithm[hashTypes.Length];

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
            foreach (IHashAlgorithm hasher in hashers)
            {
               hasher.Dispose();
            }
         }

         return result;
      }

   }
}
