using NetBox.Serialization.Core;
using Xunit;
using System.Collections;
using System.Collections.Generic;

namespace NetBox.Tests.Serialization
{
   public class NodeTest
   {
      [Fact]
      public void Discover_OneLevelSimplePropAndMember()
      {
         var node = new Node(typeof(OneLevelSimpleProps));

         Assert.Equal(8, node.Children.Count);
         Assert.Equal(NodeType.Container, node.NodeType);
         Assert.Equal(NodeType.Simple, node.Children[0].NodeType);
         Assert.Equal(NodeType.Simple, node.Children[1].NodeType);
         Assert.Equal(NodeType.Collection, node.Children[2].NodeType);
         Assert.Equal(NodeType.Collection, node.Children[3].NodeType);
         Assert.Equal(NodeType.Collection, node.Children[4].NodeType);
         Assert.Equal(NodeType.NotSupported, node.Children[5].NodeType);
         Assert.Equal(NodeType.NotSupported, node.Children[6].NodeType);
         Assert.Equal(NodeType.Collection, node.Children[7].NodeType);
      }
   }

#pragma warning disable CS0649
   class OneLevelSimpleProps
   {
      public string SP { get; }     // 0

      public string sm;             // 1

      public string[] sar;          // 2

      public string[][] smar;       // 3 not supported

      public IEnumerable<int> itie; // 4

      public IEnumerable ie;        // 5 - not supported

      public ICollection ic;        // 6 - not supported

      public ICollection<int> ict;  // 7
   }
#pragma warning restore CS0649
}
