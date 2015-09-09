// ReSharper disable once CheckNamespace
namespace System
{
   public static class TypeExtensions
   {
      /// <summary>
      /// More reliable way to determine if a type is Enum, especially in a portable reflection
      /// </summary>
      public static bool IsEnum(this Type t)
      {
         if(t == null) return false;

         try
         {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
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
