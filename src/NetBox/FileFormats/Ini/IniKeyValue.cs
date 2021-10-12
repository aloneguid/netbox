﻿namespace NetBox.FileFormats.Ini
{
   using System;

   internal class IniKeyValue : IniEntity
   {
      public const string KeyValueSeparator = "=";

      public IniKeyValue(string key, string value, string comment)
      {
         if(key == null) throw new ArgumentNullException(nameof(key));
         Key = key;
         Value = value;
         Comment = comment == null ? null : new IniComment(comment);
      }

      public string Key { get; }

      public string Value { get; set; }

      public IniComment Comment { get; }

      public static IniKeyValue FromLine(string line, bool parseInlineComments)
      {
         int idx = line.IndexOf(KeyValueSeparator, StringComparison.CurrentCulture);
         if(idx == -1) return null;

         string key = line.Substring(0, idx).Trim();
         string value = line.Substring(idx + 1).Trim();
         string comment = null;

         if (parseInlineComments)
         {
            idx = value.LastIndexOf(IniComment.CommentSeparator, StringComparison.CurrentCulture);
            if (idx != -1)
            {
               comment = value.Substring(idx + 1).Trim();
               value = value.Substring(0, idx).Trim();
            }
         }

         return new IniKeyValue(key, value, comment);
      }

      public override string ToString()
      {
         return $"{Value}";
      }
   }
}
