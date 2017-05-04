using NetBox.Application;
using NetBox.Model;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// Byte array extensions methods
   /// </summary>
   public static class ByteArrayExtensions
   {
      private static readonly char[] LowerCaseHexAlphabet = "0123456789abcdef".ToCharArray();
      private static readonly char[] UpperCaseHexAlphabet = "0123456789ABCDEF".ToCharArray();

      /// <summary>
      /// Converts byte array to hexadecimal string
      /// </summary>
      public static string ToHexString(this byte[] bytes)
      {
         return ToHexString(bytes, true);
      }

      private static string ToHexString(this byte[] bytes, bool lowerCase)
      {
         if (bytes == null) return null;

         char[] alphabet = lowerCase ? LowerCaseHexAlphabet : UpperCaseHexAlphabet;

         int len = bytes.Length;
         char[] result = new char[len * 2];

         int i = 0;
         int j = 0;

         while (i < len)
         {
            byte b = bytes[i++];
            result[j++] = alphabet[b >> 4];
            result[j++] = alphabet[b & 0xF];
         }

         return new string(result);
      }

      /// <summary>
      /// Calculates a hash on byte array
      /// </summary>
      public static byte[] GetHash(this byte[] bytes, HashType hashType)
      {
         return Hashing.GetHash(bytes, hashType);
      }

      /// <summary>
      /// Concatenates two array into a new one
      /// </summary>
      /// <typeparam name="T">Array element type</typeparam>
      /// <param name="left">Left part of the new array</param>
      /// <param name="right">Right part of the new array</param>
      /// <returns>New array object</returns>
      public static T[] Concat<T>(this T[] left, T[] right)
      {
         if (right == null) return left;

         T[] result = new T[left.Length + right.Length];
         Array.Copy(left, result, left.Length);
         Array.Copy(right, 0, result, left.Length, right.Length);
         return result;
      }

      #region [ GZip ]

      /// <summary>
      /// Gzips sequence of bytes
      /// </summary>
      public static byte[] Gzip(this byte[] data)
      {
         return Compressor.Compress(data);
      }

      /// <summary>
      /// Ungzips sequence of bytes
      /// </summary>
      public static byte[] Ungzip(this byte[] data)
      {
         return Compressor.Decompress(data);
      }

      /// <summary>
      /// Checks the beginning of the byte array for a GZIP header
      /// </summary>
      public static bool IsGzipped(this byte[] data)
      {
         return Compressor.IsGzipped(data);
      }

      #endregion
   }
}
