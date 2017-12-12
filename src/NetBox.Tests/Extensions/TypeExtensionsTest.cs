using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NetBox.Extensions;
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

      [Fact]
      public void Get_simple_class_public_properties()
      {
         PropertyInfo[] pis = typeof(FlatClass).GetHierarchyPublicProperties();

         Assert.Equal(1, pis.Length);
      }

      [Fact]
      public void Get_derived_class_public_properties()
      {
         PropertyInfo[] pis = typeof(DerivedClass).GetHierarchyPublicProperties();

         Assert.Equal(2, pis.Length);
      }

      [Fact]
      public void Get_simple_interface_public_properties()
      {
         PropertyInfo[] pis = typeof(FlatInterface).GetHierarchyPublicProperties();

         Assert.Equal(1, pis.Length);
      }

      [Fact]
      public void Get_derived_interface_public_properties()
      {
         PropertyInfo[] pis = typeof(DerivedInterface).GetHierarchyPublicProperties();

         Assert.Equal(2, pis.Length);
      }


      public class FlatClass
      {
         public int C1 { get; }
      }

      public class DerivedClass : FlatClass
      {
         public int C2 { get; }
      }

      public interface FlatInterface
      {
         int C1 { get; }
      }

      public interface DerivedInterface : FlatInterface
      {
         int C2 { get; }
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
