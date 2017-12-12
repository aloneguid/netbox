using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBox.Extensions;
using Xunit;

namespace NetBox.Tests.Extensions
{
   public class ListExtensionsTest
   {
      [Fact]
      public void Addrange_array_to_arraylist()
      {
         IList ar = new ArrayList();
         ar.AddRange(new[] { 1, 2, 3 });

         Assert.Equal(3, ar.Count);
      }

      [Fact]
      public void Addrange_Ienumerable_to_arraylist()
      {
         IList ar = new ArrayList();
         ar.AddRange((IEnumerable)(new[] { 1, 2, 3 }));

         Assert.Equal(3, ar.Count);
      }

      [Fact]
      public void Addrange_array_to_generic_list()
      {
         List<int> ar = new List<int>();
         ar.AddRange(new[] { 1, 2, 3 });
      }

   }
}
