using NetBox.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetBox.Tests.Caching
{
   public class LazyVarTest
   {
      [Fact]
      public async Task I_can_get_initial_value()
      {
         var lv = new LazyVar<string>(TimeSpan.FromSeconds(1), () =>
         {
            return Task.FromResult(DateTime.UtcNow.ToString());
         });

         string value = await lv.GetValueAsync();
         Assert.NotNull(value);
      }

      [Fact]
      public void TimeSpanZero_returns_same_value()
      {
         int i = 0;

         var lv = new LazyVar<int>(TimeSpan.Zero, () => ++i);

         Assert.Equal(1, lv.GetValue());
         Assert.Equal(1, lv.GetValue());
      }
   }
}
