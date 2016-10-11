using System.Collections.Generic;
using System.Text;
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
#if !NETSTANDARD
      private static readonly JsonSerialiser Json = new JsonSerialiser();
#endif

      #region [ General ]

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

      #endregion

      #region [ Seek and Read ]

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

      #endregion

      #region [ Stream Conversion ]

      /// <summary>
      /// Reads all stream in memory and returns as byte array
      /// </summary>
      public static byte[] ToByteArray(this Stream stream)
      {
         if(stream == null) return null;
         using(var ms = new MemoryStream())
         {
            stream.CopyTo(ms);
            return ms.ToArray();
         }
      }

      /// <summary>
      /// Converts the stream to string using specified encoding. This is done by reading the stream into
      /// byte array first, then applying specified encoding on top.
      /// </summary>
      public static string ToString(this Stream stream, Encoding encoding)
      {
         if(stream == null) return null;
         if(encoding == null) throw new ArgumentNullException(nameof(encoding));

         using (StreamReader reader = new StreamReader(stream, encoding))
         {
            return reader.ReadToEnd();
         }
      }

      #endregion

      #region [ Hashing ]

      /// <summary>
      /// Calculates hash on input stream.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="hashType">Hash type to calculate.</param>
      /// <returns>Hexadecimal representation of byte array as string</returns>
      public static string GetHash(this Stream stream, HashType hashType)
      {
         byte[] hash = Hashing.GetHash(stream, hashType);
         return hash.ToHexString();
      }

      /// <summary>
      /// Calculates hash on input stream.
      /// </summary>
      /// <param name="stream">Input stream.</param>
      /// <param name="hashType">Hash type to calculate.</param>
      /// <returns>Hash</returns>
      public static byte[] GetHashBytes(this Stream stream, HashType hashType)
      {
         return Hashing.GetHash(stream, hashType);
      }


      #endregion

#if !NETSTANDARD
      #region [ GZip ]

      /// <summary>
      /// GZips source stream into a target stream
      /// </summary>
      public static void Gzip(this Stream inputStream, Stream outputStream)
      {
         if(inputStream == null) return;
         if(outputStream == null) throw new ArgumentNullException(nameof(outputStream));

         Compressor.Compress(inputStream, outputStream);
      }

      /// <summary>
      /// UnGZips source stream into a target stream
      /// </summary>
      public static void Ungzip(this Stream inputStream, Stream outputStream)
      {
         if(inputStream == null) return;
         if(outputStream == null) throw new ArgumentNullException(nameof(outputStream));

         Compressor.Decompress(inputStream, outputStream);
      }

      #endregion
#endif

      #region [ Serialization ]

#if !NETSTANDARD

      /// <summary>
      /// Deserialise stream into a JSON object
      /// </summary>
      public static T ReadAsJsonObject<T>(this Stream stream, Encoding encoding)
      {
         if (stream == null || encoding == null) return default(T);

         string s = ToString(stream, encoding);
         return Json.Deserialise<T>(s);
      }

      /// <summary>
      /// Deserialise stream into a JSON object
      /// </summary>
      public static object ReadAsJsonObject(this Stream stream, Encoding encoding, Type t)
      {
         if (stream == null || encoding == null) return null;

         string s = ToString(stream, encoding);
         return Json.Deserialise(s, t);
      }

#endif

   #endregion
   }
}
