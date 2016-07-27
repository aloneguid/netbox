using System.Collections.Generic;
using System.Collections.Generics;
using System.Linq;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class EnumerableExtensionsTest
   {
      [Test]
      public void Chunk_LongSequence_Splits()
      {
         string[] sequence = {"one", "two", "three", "four", "five"};

         List<IEnumerable<string>> chunks = sequence.Chunk(2).ToList();

         Assert.AreEqual(3, chunks.Count);
         Assert.AreEqual(2, chunks[0].Count());
         Assert.AreEqual(2, chunks[1].Count());
         Assert.AreEqual(1, chunks[2].Count());
      }
   }
}
