using NetBox.Application;

namespace System
{
   /// <summary>
   /// Guid extensions
   /// </summary>
   public static class GuidExtensions
   {
      /// <summary>
      /// Returns shortest possible representation of guid as string i.e. 20 characters instead of
      /// 36 comparing to built-int ToString() representation. It uses ASCII86 encoding to achieve this.
      /// See <see cref="StringExtensions.FromShortestGuid(string)"/> to convert back to Guid.
      /// </summary>
      public static string ToShortest(this Guid g)
      {
         if (g == null) return null;
         return Ascii85.Instance.Encode(g.ToByteArray(), false);
      }
   }
}
