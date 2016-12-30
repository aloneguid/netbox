using NetBox.Serialization.Core;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class NodeTest
   {
      [Fact]
      public void Discover_OneLevelSimplePropAndMember()
      {
         var node = new Node(typeof(OneLevelSimpleProps));

         Assert.Equal(2, node.Children.Count);
         Assert.Equal(NodeType.Container, node.NodeType);
         Assert.Equal(NodeType.Simple, node.Children[0].NodeType);
         Assert.Equal(NodeType.Simple, node.Children[1].NodeType);
      }
   }

   class OneLevelSimpleProps
   {
      public string SP { get; }

      public string sm;
   }
}
