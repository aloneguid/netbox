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
   }
}
