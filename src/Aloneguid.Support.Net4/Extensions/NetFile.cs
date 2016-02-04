using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloneguid.Support.Extensions
{
   /// <summary>
   /// <see cref="File"/> extensions. Due to the fact <see cref="File"/> is a static class and cannot be extended
   /// with extension methods this is implemented a new static class.
   /// </summary>
   public static class NetFile
   {
      /// <summary>
      /// Gzips the file
      /// </summary>
      /// <param name="filePath">Path to the file to gzip</param>
      /// <param name="targetPath">Target file name for the gzipped file. Optional and when nul creates a new 
      /// file in the same folder appending the .gz extension. For example mytextfile.txt => mytextfile.txt.gz</param>
      public static void Gzip(string filePath, string targetPath)
      {
         throw new NotImplementedException();
      }
   }
}
