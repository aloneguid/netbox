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
   }
}
