using Aloneguid.Support.Model;
#if NETFULL
using System.Reflection;
#endif

namespace System
{
   /// <summary>
   /// Enum extensions methods
   /// </summary>
   public static class EnumExtensions
   {
#if NETFULL

      /// <summary>
      /// Gets attribute value for enums marked with <see cref="EnumTagAttribute"/>
      /// </summary>
      /// <param name="enumValue">Enumeration value</param>
      /// <returns>Tag if enum member is marked, otherwise null</returns>
      public static EnumTagAttribute GetEnumTag(this Enum enumValue)
      {
         Type t = enumValue.GetType();
         string memberName = enumValue.ToString();

         MemberInfo[] infos = t.GetMember(memberName);
         if(infos != null && infos.Length > 0)
         {
            Attribute attr = infos[0].GetCustomAttribute(typeof(EnumTagAttribute), false);
            if (attr != null) return (EnumTagAttribute)attr;
         }

         return null;
      }

#endif

   }
}
