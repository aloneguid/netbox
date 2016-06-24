using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class GuidExtensionsTest
   {
      [Test]
      public void Guid_ToShort_ConvertsBack()
      {
         Guid g = Guid.NewGuid();
         string shortGuid = g.ToShortest();

         Guid g1 = shortGuid.FromShortestGuid();
         Assert.AreEqual(g, g1);
      }
   }
}
