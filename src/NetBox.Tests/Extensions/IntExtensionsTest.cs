using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBox.Extensions;
using Xunit;

namespace NetBox.Tests.Extensions
{
   public class IntExtensionsTest
   {
      [Fact]
      public void Number_to_seconds()
      {
         Assert.Equal(TimeSpan.FromSeconds(5), 5.Seconds());
      }

      [Fact]
      public void Number_to_minutes()
      {
         Assert.Equal(TimeSpan.FromMinutes(3), 3.Minutes());
      }

      [Fact]
      public void Number_to_hours()
      {
         Assert.Equal(TimeSpan.FromHours(6), 6.Hours());
      }
   }
}
