using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetBox.Tests.Extensions
{
   public class EnumerableExTest
   {
      [Fact]
      public void MultiIterate_Null_Empty()
      {
         List<Tuple<string, string>> result = EnumerableEx.MultiIterate<string, string>(null, null).ToList();

         Assert.Equal(0, result.Count);
      }

      [Fact]
      public void MultiIterate_DifferentTypesSameLength_Iterates()
      {
         var result = EnumerableEx.MultiIterate(
            new string[] { "1", "2" },
            new int[] { 3, 4 }).ToList();

         Assert.Equal(2, result.Count);
         Assert.Equal("1", result[0].Item1);
         Assert.Equal(3, result[0].Item2);
         Assert.Equal("2", result[1].Item1);
         Assert.Equal(4, result[1].Item2);
      }

      [Fact]
      public void MultiIterate_SameTypesDifferentLength_Iterates()
      {
         var result = EnumerableEx.MultiIterate(
            new string[] { "1" },
            new int[] { 1, 2, 3 }).ToList();

         Assert.Equal(1, result.Count);
         Assert.Equal("1", result[0].Item1);
         Assert.Equal(1, result[0].Item2);
      }
   }
}
