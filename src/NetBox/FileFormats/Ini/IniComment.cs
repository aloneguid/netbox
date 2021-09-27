﻿namespace NetBox.FileFormats.Ini
{
   class IniComment : IniEntity
   {
      public const string CommentSeparator = ";";

      public IniComment(string value)
      {
         Value = value;
      }

      public string Value { get; set; }

      public override string ToString() => Value;
   }
}
