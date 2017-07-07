using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace NetBox.Tests.Extensions
{
   public class TypeExtensionsTest
   {
      [Theory, MemberData(nameof(IsSimple_Variable_Data))]
      public void IsSimple_Variable(object obj, bool isSimple)
      {
         bool simple = obj.GetType().IsSimple();

         Assert.True(simple == isSimple, $"{obj.GetType()} is supposed to be simple: {isSimple}");
      }

      [Fact]
      public void Default_value_for_ref_type_is_null()
      {
         Assert.Null(typeof(TypeExtensionsTest).CreateDefaultValue());
      }

      [Fact]
      public void Default_value_for_value_type_is_zero()
      {
         Assert.Equal(0, typeof(int).CreateDefaultValue());
      }

      public static IEnumerable<object[]> IsSimple_Variable_Data
      {
         get
         {
            // Or this could read from a file. :)
            return new[]
            {
                new object[] { "xUnit", true },
                new object[] { 1, true },
                new object[] { Guid.NewGuid(), true },
                new object[] { TimeSpan.FromSeconds(1), true },
                new object[] { DateTime.UtcNow, true },
                new object[] { new SimpleClass(), false }
            };
         }
      }

      public class SimpleClass
      {

      }
   }
}
