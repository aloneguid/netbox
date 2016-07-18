using System;

namespace Aloneguid.Support.Model
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
   }
}
