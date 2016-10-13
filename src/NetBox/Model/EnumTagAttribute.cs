using System;
#if NETFULL
using System.Reflection;
#endif

namespace NetBox.Model
{
   /// <summary>
   /// Tags enums for assigning extra data such as alternative ID, Description etc.
   /// </summary>
   [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
   public sealed class EnumTagAttribute : Attribute
   {
      /// <summary>
      /// Alternative string version of ID
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      /// Display name
      /// </summary>
      public string DisplayName { get; set; }

      /// <summary>
      /// Creates an instance
      /// </summary>
      public EnumTagAttribute()
      {

      }

      /// <summary>
      /// Creates an instance
      /// </summary>
      /// <param name="id">Alternative string version of ID</param>
      /// <param name="displayName">Display name</param>
      public EnumTagAttribute(string id = null, string displayName = null)
      {
         Id = id;
         DisplayName = displayName;
      }

#if NETFULL

      /// <summary>
      /// Finds enum member by tagged ID
      /// </summary>
      /// <typeparam name="T">Enumeration type</typeparam>
      /// <param name="id">Tagged id</param>
      /// <returns>Enumeration member if gived ID is found, otherwise null</returns>
      public static T? GetById<T>(string id) where T : struct
      {
         Type t = typeof(T);

         if (!t.IsEnum) throw new ArgumentException("T must be an enum");

         string[] names = Enum.GetNames(t);

         foreach(string memberName in names)
         {
            MemberInfo[] member = t.GetMember(memberName);
            if (member == null || member.Length == 0) continue;

            EnumTagAttribute attribute = member[0].GetCustomAttribute(typeof(EnumTagAttribute), false) as EnumTagAttribute;
            if (attribute == null) continue;

            if(attribute.Id == id)
            {
               return (T)Enum.Parse(t, memberName);
            }
         }

         return null;

      }

#endif
   }
}
