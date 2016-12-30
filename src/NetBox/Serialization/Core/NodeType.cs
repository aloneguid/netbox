namespace NetBox.Serialization.Core
{
   /// <summary>
   /// Type of the node
   /// </summary>
   public enum NodeType
   {
      Simple,

      Container,

      Collection,

      /// <summary>
      /// The node belongs to the type but it's too complex and is not supported. Examples are:
      /// - multidimensional arrays
      /// </summary>
      NotSupported
   }
}
