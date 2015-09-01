// ReSharper disable once CheckNamespace
namespace System
{
   public static class TypeExtensions
   {
      public static bool IsEnum(this Type t)
      {
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
   }
}
