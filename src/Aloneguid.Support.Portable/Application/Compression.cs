using System;
using System.IO;
using System.IO.Compression;

namespace Aloneguid.Support.Application
{
   static class Compression
   {
      public static void Compress(Stream source, Stream destination)
      {
         if(source == null) throw new ArgumentNullException(nameof(source));
         if(destination == null) throw new ArgumentNullException(nameof(destination));

         using(var compressor = new GZipStream(destination, CompressionLevel.Optimal, true))
         {
            source.CopyTo(compressor);
            compressor.Flush();
         }
      }

      public static void Decompress(Stream source, Stream destination)
      {
         if(source == null) throw new ArgumentNullException(nameof(source));
         if(destination == null) throw new ArgumentNullException(nameof(destination));

         using(var decompressor = new GZipStream(source, CompressionMode.Decompress, true))
         {
            decompressor.CopyTo(destination);
            destination.Flush();
         }
      }

      public static byte[] Compress(byte[] source)
      {
         using(var sourceStream = new MemoryStream(source))
         {
            using(var destinationStream = new MemoryStream())
            {
               Compress(sourceStream, destinationStream);
               return destinationStream.ToArray();
            }
         }
      }

      public static byte[] Decompress(byte[] source)
      {
         using(var sourceStream = new MemoryStream(source))
         {
            using(var destinationStream = new MemoryStream())
            {
               Decompress(sourceStream, destinationStream);
               return destinationStream.ToArray();
            }
         }
      }
   }
}
