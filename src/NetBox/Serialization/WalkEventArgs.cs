namespace NetBox.Serialization
{
   /// <summary>
   /// Walk event arguments
   /// </summary>
   public struct WalkEventArgs
   {
      public string Name;

      public NodeInfo Node;

      public object Parent;

      /// <summary>
      /// Member level starting with 0 where 0 is the top level
      /// </summary>
      public int Level;

      public WalkEventArgs(string name, NodeInfo node, object parent, int level)
      {
         Name = name;
         Node = node;
         Parent = parent;
         Level = level;
      }
   }
}
