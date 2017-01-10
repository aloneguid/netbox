namespace System.IO
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
         if (filePath == null) throw new ArgumentNullException(nameof(filePath));
         if (!File.Exists(filePath)) throw new ArgumentException("file does not exist", nameof(filePath));

         if (targetPath == null) targetPath = filePath + ".gz";

         using (FileStream source = File.OpenRead(filePath))
         {
            using (Stream target = File.Create(targetPath))
            {
               source.Gzip(target);
            }
         }
      }

      /// <summary>
      /// Ungzips the file
      /// </summary>
      /// <param name="filePath">Path to the file to ungzip</param>
      /// <param name="targetPath">Target file for the ungzipped file. Optional. When null source file must end with
      /// .gz extension and target file will be decompressed to a file without it. For example mytextfile.txt.gz => mytextfile.txt</param>
      public static void Ungzip(string filePath, string targetPath)
      {
         if (filePath == null) throw new ArgumentNullException(nameof(filePath));
         if (!File.Exists(filePath)) throw new ArgumentException("file does not exist", nameof(filePath));

         if (targetPath == null)
         {
            if(!filePath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
            {
               throw new ArgumentException("value is null and source file is not ending with .gz extension, cannot guess target filename", nameof(targetPath));
            }

            targetPath = filePath.Substring(0, filePath.Length - 3);
         }

         using (FileStream source = File.OpenRead(filePath))
         {
            using (Stream target = File.Create(targetPath))
            {
               source.Ungzip(target);
            }
         }
      }
   }
}