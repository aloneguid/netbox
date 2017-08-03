using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System
{
   /// <summary>
   /// <see cref="Type"/> extensions
   /// </summary>
   public static class TypeExtensions
   {
      /// <summary>
      /// More reliable way to determine if a type is Enum
      /// </summary>
      public static bool IsEnum(this Type t)
      {
         if(t == null) return false;

         try
         {
            Enum.GetUnderlyingType(t);
            return true;
         }
         catch(ArgumentException)
         {
            return false;
         }
      }

      /// <summary>
      /// Deterimites if type is "simple". It's hard to describe what a simple type is, but this is used mostly in serialization and
      /// says whether a type has to be expanded (not simple) or represented as a value.
      /// </summary>
      /// <param name="t">The type</param>
      /// <returns>True if a type is siimple</returns>
      public static bool IsSimple(this Type t)
      {
         if (t == null) return true;

         return
            t == typeof(bool) ||
            t == typeof(byte) ||
            t == typeof(sbyte) ||
            t == typeof(char) ||
            t == typeof(decimal) ||
            t == typeof(double) ||
            t == typeof(float) ||
            t == typeof(int) ||
            t == typeof(uint) ||
            t == typeof(long) ||
            t == typeof(ulong) ||
            t == typeof(short) ||
            t == typeof(TimeSpan) ||
            t == typeof(DateTime) ||
            t == typeof(Guid) ||
            t == typeof(string);
      }

      /// <summary>
      /// Creates the default value for type. It's null for reference type and 0 for int, float etc.
      /// </summary>
      /// <param name="t">The value.</param>
      /// <returns>Value</returns>
      public static object CreateDefaultValue(this Type t)
      {
         if (t.GetTypeInfo().IsValueType)
            return Activator.CreateInstance(t);

         return null;
      }

      /// <summary>
      /// Gets the assembly this type is in
      /// </summary>
      /// <param name="t">The type.</param>
      /// <returns></returns>
      public static Assembly GetAssembly(this Type t)
      {
         return t.GetTypeInfo().Assembly;
      }

      /// <summary>
      /// Gets the product version (set by [assembly:Version] attribute)
      /// </summary>
      public static Version ProductVersion(this Type t)
      {
         return GetAssembly(t).GetName().Version;
      }

      /// <summary>
      /// Gets the file version (set by [assembly:FileVersion] attribute)
      /// </summary>
      /// <param name="t">A type within the assembly</param>
      /// <returns></returns>
      public static Version FileVersion(this Type t)
      {
         var fva = GetAssembly(t).CustomAttributes.First(a => a.AttributeType == typeof(AssemblyFileVersionAttribute));
         CustomAttributeTypedArgument varg = fva.ConstructorArguments[0];
         string fileVersion = (string)varg.Value;
         return new Version(fileVersion);
      }

      /// <summary>
      /// Reads embedded resource file which lies next to a type specified in TTypeNextToFile 
      /// </summary>
      /// <param name="type">Type which resides in the same folder as the resource</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static Stream GetSameFolderEmbeddedResourceFile(this Type type, string fileName)
      {
         string resourceName = $"{type.Namespace}.{fileName}";

         return GetAssembly(type).GetManifestResourceStream(resourceName);
      }

      /// <summary>
      /// Reads embedded resource file as text
      /// </summary>
      /// <param name="type">Type which resides in the same folder as the resource</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static string GetSameFolderEmbeddedResourceFileAsText(this Type type,
         string fileName)
      {
         using (Stream src = GetSameFolderEmbeddedResourceFile(type, fileName))
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
      /// <param name="type">Type which resides in the same folder as the resource</param>
      /// <param name="fileName">name of the file, i.e. "myresource.txt"</param>
      /// <returns>File stream if it exists, otherwise null</returns>
      public static string[] GetSameFolderEmbeddedResourceFileAsLines<TTypeNextToFile>(this Type type,
         string fileName)
      {
         var result = new List<string>();
         using (Stream rawStream = GetSameFolderEmbeddedResourceFile(type, fileName))
         {
            if (rawStream == null) return null;
            using (var reader = new StreamReader(rawStream))
            {
               string line;
               while ((line = reader.ReadLine()) != null)
               {
                  if (!string.IsNullOrEmpty(line)) result.Add(line.Trim());
               }
            }
         }
         return result.ToArray();
      }


   }
}
