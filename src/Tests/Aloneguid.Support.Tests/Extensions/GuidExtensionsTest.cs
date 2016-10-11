using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloneguid.Support.Tests.Extensions
{
   
   public class GuidExtensionsTest
   {
      [Fact]
      public void Guid_ToShort_ConvertsBack()
      {
         Guid g = Guid.NewGuid();
         string shortGuid = g.ToShortest();

         Guid g1 = shortGuid.FromShortestGuid();
         Assert.Equal(g, g1);
      }
   }
}
