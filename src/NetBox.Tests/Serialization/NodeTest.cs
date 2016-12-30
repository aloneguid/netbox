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

   class OneLevelSimpleProps
   {
      public string SP { get; }

      public string sm;

      public string[] sar;

      public string[][] smar; //not supported

      public IEnumerable<int> itie;

      public IEnumerable ie;  //not supported

      public ICollection ic;  //not supported

      public ICollection<int> ict;
   }
}
