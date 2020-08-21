using NetBox.Model;
using System;
#if NETSTANDARD20
using System.Reflection;
#endif

namespace NetBox.Extensions
{
   /// <summary>
   /// Enum extensions methods
   /// </summary>
   public static class EnumExtensions
   {
#if !NETSTANDARD16

      /// <summary>
      /// Gets attribute value for enums marked with <see cref="EnumTagAttribute"/>
      /// </summary>
      /// <param name="enumValue">Enumeration value</param>
      /// <returns>Tag if enum member is marked, otherwise null</returns>
      public static EnumTagAttribute GetEnumTag(this object enumValue)
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
