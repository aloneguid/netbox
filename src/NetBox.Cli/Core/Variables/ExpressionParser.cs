using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace Housework.Console.Core.Variables
{
   public static partial class ExpressionParser
   {
      internal const string Delimiter = "%";
      internal const string ArgsDelimiter = ":";
      internal const string Escaped = "%%";

      public static IReadOnlyCollection<VariableDefinition> ParseVariables(string expression)
      {
         Parser<string> escapedDelimiter = Parse.String("%%").Text();
         Parser<string> expressionDelimiter = Parse.String("%").Text();
         Parser<string> commandArgsDelimiter = Parse.String(":").Text();

         //terminators
         Parser<string> simpleLiteral = Parse.AnyChar.Except(expressionDelimiter).Many().Text();
         Parser<string> commandLiteral = Parse.AnyChar.Except(expressionDelimiter).Except(commandArgsDelimiter).Many().Text();

         Parser<VariableDefinition> variableChunk =
            from throwAway in escapedDelimiter.Or(simpleLiteral).Many()
            from l in expressionDelimiter
            from vn in escapedDelimiter.Or(commandLiteral).AtLeastOnce()
            from d in Parse.String(":").Many()
            from vv in escapedDelimiter.Or(commandLiteral).Many()
            from r in expressionDelimiter
            select new VariableDefinition(string.Concat(vn), string.Concat(vv));

         Parser<IEnumerable<VariableDefinition>> variablesParser =
            from vd in variableChunk.Many()
            select vd;

         List<VariableDefinition> vds = variablesParser.Parse(expression).ToList();

         return vds.Distinct().ToList();
      }
   }
}
