using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Reflection
{
   /// <summary>
   /// <see cref="Assembly"/> extension methods
   /// </summary>
   public static class AssemblyExtensions
   {

      /// <summary>
      /// Gets the product version (set by [assembly:Version] attribute)
      /// </summary>
      public static Version ProductVersion(this Assembly asm)
      {
         return asm.GetName().Version;
      }

      /// <summary>
      /// Reads embedded resource file which lies next to a type specified in TTypeNextToFile 
      /// </summary>
      /// <param name="assembly">Assembly where the resource file resides, usually it's Assembly.GetExecutingAssembly()</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static Stream GetSameFolderEmbeddedResourceFile<TTypeNextToFile>(this Assembly assembly, string fileName)
      {
         string resourceName = $"{typeof(TTypeNextToFile).Namespace}.{fileName}";

         return assembly.GetManifestResourceStream(resourceName);
      }

      /// <summary>
      /// Reads embedded resource file as text
      /// </summary>
      /// <typeparam name="TTypeNextToFile">This type must reside in the same folder as resource file</typeparam>
      /// <param name="assembly">Assembly where the resource file resides, usually it's Assembly.GetExecutingAssembly()</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static string GetSameFolderEmbeddedResourceFileAsText<TTypeNextToFile>(this Assembly assembly,
         string fileName)
      {
         using (Stream src = GetSameFolderEmbeddedResourceFile<TTypeNextToFile>(assembly, fileName))
         {
            if (src == null) return null;

            using (var reader = new StreamReader(src))
            {
               return reader.ReadToEnd();
            }
         }
      }

      /// <summary>
      /// Reads embedded resource file as array of lines
      /// </summary>
      /// <typeparam name="TTypeNextToFile">This type must reside in the same folder as resource file</typeparam>
      /// <param name="assembly">Assembly where the resource file resides, usually it's Assembly.GetExecutingAssembly()</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static string[] GetSameFolderEmbeddedResourceFileAsLines<TTypeNextToFile>(this Assembly assembly,
         string fileName)
      {
         var result = new List<string>();
         using (Stream rawStream = GetSameFolderEmbeddedResourceFile<TTypeNextToFile>(assembly, fileName))
         {
            if (rawStream == null) return null;
            using (var reader = new StreamReader(rawStream))
            {
               string line;
               while((line = reader.ReadLine()) != null)
               {
                  if(!string.IsNullOrEmpty(line)) result.Add(line.Trim());
               }
            }
         }
         return result.ToArray();
      }

      /// <summary>
      /// Gets the file version (set by [assembly:FileVersion] attribute)
      /// </summary>
      /// <param name="asm"></param>
      /// <returns></returns>
      public static Version FileVersion(this Assembly asm)
      {
         var fva = asm.CustomAttributes.First(a => a.AttributeType == typeof(AssemblyFileVersionAttribute));
         CustomAttributeTypedArgument varg = fva.ConstructorArguments[0];
         string fileVersion = (string)varg.Value;
         return new Version(fileVersion);
      }
   }
}

