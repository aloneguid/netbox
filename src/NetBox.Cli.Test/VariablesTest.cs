using System;
using System.Collections.Generic;
using System.Linq;
using Housework.Console.Core.Variables;
using Housework.Core.Variables;
using Xunit;

namespace Housework.Test
{
   public class VariablesTest
   {
      [Fact]
      public void Parse_simple_variable()
      {
         Assert.Equal("y2019", VariablesEngine.Expand("y%date:yyyy%", null));
      }

      [Fact]
      public void Parse_no_variables()
      {
         Assert.Equal("none", VariablesEngine.Expand("none", null));
      }

      [Fact]
      public void Parse_variations()
      {
         IReadOnlyCollection<VariableDefinition> vars = ExpressionParser.ParseVariables("y%date:yyyy%,m%date:mm%,%date%");

         Assert.Equal(3, vars.Count);
      }

      [Fact]
      public void Parse_cmd_and_args()
      {
         IReadOnlyCollection<VariableDefinition> vars = ExpressionParser.ParseVariables("pre%the:one%");

         Assert.Equal(1, vars.Count);
         Assert.Equal("the", vars.First().Name);
         Assert.Equal("one", vars.First().Args);
      }

      [Fact]
      public void Parse_command_only()
      {
         IReadOnlyCollection<VariableDefinition> vars = ExpressionParser.ParseVariables("pre%the%");

         Assert.Equal(1, vars.Count);
         Assert.Equal("the", vars.First().Name);
         Assert.Equal("", vars.First().Args);
      }

      [Fact]
      public void Parse_escapes_in_text()
      {
         IReadOnlyCollection<VariableDefinition> vars = ExpressionParser.ParseVariables("p%%r%%e%the%");
         Assert.Equal(1, vars.Count);
      }
   }
}