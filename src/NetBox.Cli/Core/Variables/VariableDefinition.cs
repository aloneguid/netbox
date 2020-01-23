using System;

namespace Housework.Console.Core.Variables
{
   public sealed class VariableDefinition : IEquatable<VariableDefinition>
   {
      public VariableDefinition(string name, string args)
      {
         Name = name;
         Args = args;
      }

      public string Name { get; private set; }

      public string Args { get; private set; }

      public string ArgsDelim => Args == string.Empty ? string.Empty : ExpressionParser.ArgsDelimiter.ToString();

      public string Value => $"{Name}{ArgsDelim}{Args}";

      public string SearchValue => $"{ExpressionParser.Delimiter}{Value}{ExpressionParser.Delimiter}";

      public bool Equals(VariableDefinition other)
      {
         if (ReferenceEquals(other, null)) return false;
         return other.Value.Equals(Value);
      }

      public override string ToString()
      {
         return Value;
      }
   }
}