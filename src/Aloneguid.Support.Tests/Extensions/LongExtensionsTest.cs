using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class LongExtensionsTest
   {
      [TestCase(1024, "1.00 KiB")]
      [TestCase(0, "0")]
      public void ToFileSizeString_Variable_Variable(long input, string expected)
      {
         Assert.AreEqual(expected, input.ToFileSizeString());
      }

      [TestCase(1024, "1.02 KB")]
      [TestCase(0, "0")]
      public void ToFileSizeUiString_Variable_Variable(long input, string expected)
      {
         Assert.AreEqual(expected, input.ToFileSizeUiString());
      }
   }
}
