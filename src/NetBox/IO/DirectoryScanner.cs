using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NetBox.Model;

namespace NetBox.IO
{
   class DirectoryScanner
   {
      private readonly DirectoryInfo _baseDirectory;
      private const string SingleCharRegex = "[^/\\\\]{1}";    //?
      private const string MultipleCharsRegex = "[^/\\\\]+";   //*
      private const string AllIncludingDirectorySeparatorsRegex = ".*";

      public DirectoryScanner(DirectoryInfo baseDirectory)
      {
         _baseDirectory = baseDirectory ?? throw new ArgumentNullException(nameof(baseDirectory));
      }

      public IReadOnlyCollection<string> Scan(FileSearchOptions options)
      {
         IEnumerable<string> allFiles = Directory.EnumerateFiles(_baseDirectory.FullName, "*", SearchOption.AllDirectories);
         Regex rgx = CreateExpression(options.IncludePatterns.First());

         return allFiles
            .Where(f => rgx.IsMatch(f))
            .ToList();
      }

      private Regex CreateExpression(string wildcardExpression)
      {
         string regexText = wildcardExpression
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace("\\", "\\\\")
            .Replace(".", "\\.")
            .Replace("?", SingleCharRegex)
            .Replace("**", AllIncludingDirectorySeparatorsRegex)
            .Replace("*", MultipleCharsRegex);

         return new Regex(regexText, RegexOptions.Compiled | RegexOptions.Singleline);
      }

   }
}