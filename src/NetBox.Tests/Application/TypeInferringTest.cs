using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetBox.Tests.Application
{
   public class TypeInferringTest
   {
      [Theory]
      [InlineData("1,0", "System.Byte")]
      [InlineData("True,false", "System.Boolean")]
      [InlineData("1000,2,3,4", "System.Int16")]
      [InlineData("1,289878788,,4", "System.Int32")]
      [InlineData("1,,,42323", "System.Int32")]
      [InlineData("1,2,dfdsf,4", "System.String")]
      [InlineData("a,b,c,", "System.Char")]
      [InlineData("1.1,1.2,3", "System.Single")]
      [InlineData("1341341333444444444444444444444444433333333334.13333333333333333333333333333,1.2,3", "System.Double")]
      [InlineData("12/07/2017 23:50:04,12/07/2017 23:50:04,12/07/2017 23:50:04,,,", "System.DateTimeOffset")]
      public void Infer_type_from_variable_inputs(string data, string expectedType)
      {
         string[] input = data.Split(',');

         Type expected = Type.GetType(expectedType);

         Assert.Equal(expected, input.InferType(out IList parsedValues));
      }

      [Fact]
      public void Infer_and_get_values()
      {
         Type t = new string[] { "1", "2", "3" }.InferType(out IList values);

         Assert.Equal(typeof(byte), t);

         Assert.Equal(3, values.Count);
         Assert.Equal((byte)1, values[0]);
         Assert.Equal((byte)2, values[1]);
         Assert.Equal((byte)3, values[2]);
      }
   }
}
