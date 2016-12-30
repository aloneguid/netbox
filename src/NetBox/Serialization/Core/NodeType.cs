namespace NetBox.Serialization.Core
{
   /// <summary>
   /// Type of the node
   /// </summary>
   public enum NodeType
   {
      /// <summary>
      /// Simple node that has no children and just a primitive value
      /// </summary>
      Simple,

      /// <summary>
      /// A container node for other nodes
      /// </summary>
      Container,

      /// <summary>
      /// A collection
      /// </summary>
      Collection,

      /// <summary>
      /// The node belongs to the type but it's too complex and is not supported. Examples are:
      /// - multidimensional arrays
      /// </summary>
      NotSupported
   }
}
