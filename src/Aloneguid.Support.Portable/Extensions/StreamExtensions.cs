using System.Collections.Generic;
using Aloneguid.Support.Application;
using Aloneguid.Support.Model;

// ReSharper disable once CheckNamespace
namespace System.IO
{
   /// <summary>
   /// <see cref="Stream"/> extension
   /// </summary>
   public static class StreamExtensions
   {
      /// <summary>
      /// Attemps to get the size of this stream by reading the Length property, otherwise returns 0.
      /// </summary>
      public static bool TryGetSize(this Stream s, out long size)
      {
         try
         {
            size = s.Length;
            return true;
         }
         catch (NotSupportedException)
         {

         }
         catch (ObjectDisposedException)
         {

         }

         size = 0;
         return false;
      }

      /// <summary>
      /// Attemps to get the size of this stream by reading the Length property, otherwise returns 0.
      /// </summary>
      public static long? TryGetSize(this Stream s)
      {
         long size;
         if (TryGetSize(s, out size))
         {
            return size;
         }

         return null;
      }

      /// <summary>
      /// Reads the stream until a specified sequence of bytes is reached.
      /// </summary>
      /// <returns>Bytes before the stop sequence</returns>
      public static byte[] ReadUntil(this Stream s, byte[] stopSequence)
      {
         var buf = new byte[1];
         var result = new List<byte>(50);
         int charsMatched = 0;

         while (s.Read(buf, 0, 1) == 1)
         {
            byte b = buf[0];
            result.Add(b);

            if (b == stopSequence[charsMatched])
            {
               if (++charsMatched == stopSequence.Length)
               {
                  break;
               }
            }
            else
            {
               charsMatched = 0;
            }

         }
         return result.ToArray();
      }

      #region [ Hashing ]

      /// <summary>
      /// Calculates hashes on input stream.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="hashes">Hash types to calculate.</param>
      /// <returns>Array of hash strings, in the order they are passes in hashes params.</returns>
      public static string[] GetHashes(this Stream stream, params HashType[] hashes)
      {
         long streamLength;
         return GetHashes(stream, out streamLength, hashes);
      }

      /// <summary>
      /// Calculates hash on input stream.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="hashes">Hash types to calculate.</param>
      /// <param name="streamLength">Length counter.</param>
      /// <returns>Array of hash strings, in the order they are passes in hashes params.</returns>
      public static string[] GetHashes(this Stream stream, out long streamLength, params HashType[] hashes)
      {
         byte[][] hashBytes = Hashing.CalculateHashes(stream, out streamLength, hashes);

         return HashBytesToHashStrings(hashBytes);
      }

      /// <summary>
      /// Calculates hash on input stream.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="hashType">Hash type to calculate.</param>
      /// <returns></returns>
      public static string GetHash(this Stream stream, HashType hashType)
      {
         long streamLength;

         return GetHash(stream, out streamLength, hashType);
      }

      /// <summary>
      /// Calculates hash on input stream and the stream length.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="streamLength">Length counter.</param>
      /// <param name="hashType">Hash type to calculate.</param>
      /// <returns></returns>
      public static string GetHash(this Stream stream, out long streamLength, HashType hashType)
      {
         byte[][] hashBytes = Hashing.CalculateHashes(stream, out streamLength, hashType);

         if (hashBytes == null || hashBytes.Length == 0) return null;
         byte[] first = hashBytes[0];
         return first.ToHexString();
      }

      private static string[] HashBytesToHashStrings(byte[][] bytes)
      {
         if (bytes == null || bytes.Length == 0) return null;

         string[] result = new string[bytes.Length];
         for (int i = 0; i < bytes.Length; i++)
         {
            result[i] = bytes[i].ToHexString();
         }
         return result;
      }

      #endregion


   }
}
