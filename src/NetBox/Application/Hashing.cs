using System;
using System.Collections.Generic;
using System.IO;
using NetBox.Application.Cryptography;
using NetBox.Model;

namespace NetBox.Application
{
   static class Hashing
   {
      private static readonly Dictionary<HashType, IHashAlgorithm> Hashers = new Dictionary<HashType, IHashAlgorithm>();
      private const int BufferSize = 1024;

      public static byte[] GetHash(byte[] input, HashType hashType, byte[] salt)
      {
         if (input == null) return null;

         lock (Hashers)
         {
            return GetHasher(hashType, salt).ComputeHash(input, salt);
         }
      }

      public static byte[] GetHash(Stream input, HashType hashType, byte[] salt)
      {
         if (input == null) return null;

         lock(Hashers)
         {
            return GetHasher(hashType, salt).ComputeHash(input, salt);
         }
      }

      private static IHashAlgorithm GetHasher(HashType hashType, byte[] salt)
      {
         if (salt == null)
         {
            IHashAlgorithm result;
            if (!Hashers.TryGetValue(hashType, out result))
            {
               result = Hashers[hashType] = CreateHasher(hashType, null);
            }

            return result;
         }

         return CreateHasher(hashType, salt);
      }

      private static IHashAlgorithm CreateHasher(HashType hashType, byte[] salt)
      {
         return new FullHashAlgorithm(hashType, salt);
      }

      /*
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
      */

   }
}
