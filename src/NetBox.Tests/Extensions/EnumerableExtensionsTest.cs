using System.Collections.Generic;
using System.Linq;
using NetBox.Extensions;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class EnumerableExtensionsTest
   {
      [Fact]
      public void Chunk_LongSequence_Splits()
      {
         string[] sequence = {"one", "two", "three", "four", "five"};

         List<IEnumerable<string>> chunks = sequence.Chunk(2).ToList();

         Assert.Equal(3, chunks.Count);
         Assert.Equal(2, chunks[0].Count());
         Assert.Equal(2, chunks[1].Count());
         Assert.Equal(1, chunks[2].Count());
      }
   }
}
